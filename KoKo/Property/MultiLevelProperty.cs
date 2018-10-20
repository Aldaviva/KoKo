using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace KoKo.Property {

    public class MultiLevelProperty<T>: UnsettableProperty<T> {

        private readonly PropertyInstrumentingVisitor instrumentor;
        private readonly Func<Property<T>> instrumentedPropertyExpression;

        public override T Value => CachedValue;

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