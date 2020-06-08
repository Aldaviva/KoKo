using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace KoKo.Property {

    /// <summary>
    /// Property that can evaluate a chain of other nested properties to get its value. Useful if you want to listen for changes to a
    /// sub-property of a top-level property, but the top-level property may not always refer to the same instance.
    /// </summary>
    /// <typeparam name="T">The type of this property's value, which is also the type of the rightmost property in the expression
    /// chain</typeparam>
    public class MultiLevelProperty<T>: UnsettableProperty<T> {

        private readonly PropertyInstrumentingVisitor instrumentor;
        private readonly Func<Property<T>> instrumentedPropertyExpression;

        public override T Value => CachedValue;

        /// <summary>
        /// Create a property whose value is obtained from a specified property accessor chain
        /// </summary>
        /// <param name="multiLevelPropertyExpression">A function expression consisting of a chain of property reads that return a leaf property. This property's value will be taken from that leaf's property.<br/><code>new MultiLevelProperty(() => currentSession.Value.currentUser.Value.fullName)</code></param>
        public MultiLevelProperty(Expression<Func<Property<T>>> multiLevelPropertyExpression): base(default) {
            instrumentor = new PropertyInstrumentingVisitor();
            instrumentedPropertyExpression = ((Expression<Func<Property<T>>>) instrumentor.Visit(multiLevelPropertyExpression))?.Compile() ??
                                             throw new ArgumentOutOfRangeException(nameof(multiLevelPropertyExpression));

            CachedValue = instrumentedPropertyExpression().Value;

            foreach (Property instrumentedProperty in instrumentor.Properties) {
                instrumentedProperty.PropertyChanged += OnInstrumentedPropertyChanged;
            }
        }

        protected override T ComputeValue() {
            return instrumentedPropertyExpression().Value;
        }

        private void OnInstrumentedPropertyChanged(object sender, PropertyChangedEventArgs args) {
            foreach (Property instrumentedProperty in instrumentor.Properties) {
                instrumentedProperty.PropertyChanged -= OnInstrumentedPropertyChanged;
            }

            instrumentor.Reset();

            ComputeValueAndFireChangeEvents();

            foreach (Property instrumentedProperty in instrumentor.Properties) {
                instrumentedProperty.PropertyChanged += OnInstrumentedPropertyChanged;
            }
        }

    }

    internal class PropertyInstrumentingVisitor: ExpressionVisitor {

        private static readonly TypeInfo PropertyTypeInfo = typeof(Property).GetTypeInfo();
        private static readonly MethodInfo InstrumentPropertyAccessorMethod = typeof(PropertyInstrumentingVisitor).GetRuntimeMethod("InstrumentPropertyAccessor", new[] { typeof(Property) });

        private readonly List<Property> propertiesWritable = new List<Property>();
        public IEnumerable<Property> Properties => propertiesWritable;

        private readonly ISet<MemberExpression> instrumentedNodes = new HashSet<MemberExpression>();

        public Property InstrumentPropertyAccessor(Property intercepted) {
            propertiesWritable.Add(intercepted);
            return intercepted;
        }

        protected override Expression VisitMember(MemberExpression node) {
            if (!instrumentedNodes.Contains(node) && PropertyTypeInfo.IsAssignableFrom(node.Type.GetTypeInfo())) {
                MethodCallExpression interceptCall = Expression.Call(Expression.Constant(this), InstrumentPropertyAccessorMethod, node);
                UnaryExpression castCall = Expression.TypeAs(interceptCall, node.Type);
                instrumentedNodes.Add(node);
                return VisitUnary(castCall);
            } else {
                return base.VisitMember(node);
            }
        }

        /// <summary>
        /// Call this before you run <c>Visit()</c> a second time.
        /// </summary>
        public void Reset() {
            propertiesWritable.Clear();
            instrumentedNodes.Clear();
        }

    }

}