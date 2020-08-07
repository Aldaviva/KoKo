using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KoKo.Property;
using Xunit;
using Xunit.Abstractions;

namespace Test {

    public class TestSynchronizationContextProperty {

        private readonly ITestOutputHelper testOutputHelper;

        public TestSynchronizationContextProperty(ITestOutputHelper testOutputHelper) {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunsOnSpecifiedThread() {
            var storedProperty = new StoredProperty<bool>();
            var asyncPassthroughProperty = new SynchronizationContextProperty<bool>(storedProperty, SynchronizationContext.Current);
            testOutputHelper.WriteLine("Created property on thread " + Thread.CurrentThread.ManagedThreadId);

            Thread callbackThread = null;
            var barrier = new ManualResetEvent(false);

            asyncPassthroughProperty.PropertyChanged += (sender, args) => {
                callbackThread = Thread.CurrentThread;
                testOutputHelper.WriteLine("Received callback on thread " + Thread.CurrentThread.ManagedThreadId);
                barrier.Set();
            };

            Thread setterThread = null;
            Task.Run(() => {
                setterThread = Thread.CurrentThread;
                testOutputHelper.WriteLine("Updating value on thread " + Thread.CurrentThread.ManagedThreadId);
                storedProperty.Value = true;
            });

            barrier.WaitOne();

            callbackThread.ManagedThreadId.Should().NotBe(setterThread.ManagedThreadId);
        }

        [Fact]
        public void RunsOnCallingThread() {
            var storedProperty = new StoredProperty<bool>();
            var passthroughProperty = new PassthroughProperty<bool>(storedProperty);
            testOutputHelper.WriteLine($"Created property on thread {Thread.CurrentThread.ManagedThreadId}");

            Thread callbackThread = null;
            var barrier = new ManualResetEvent(false);

            passthroughProperty.PropertyChanged += (sender, args) => {
                callbackThread = Thread.CurrentThread;
                testOutputHelper.WriteLine($"Received callback on thread {callbackThread.ManagedThreadId} ({callbackThread.Name})");
                barrier.Set();
            };

            Thread setterThread = null;
            Task.Run(() => {
                setterThread = Thread.CurrentThread;
                testOutputHelper.WriteLine($"Updating value on thread {setterThread.ManagedThreadId} ({setterThread.Name})");
                storedProperty.Value = true;
            });

            barrier.WaitOne();

            callbackThread.ManagedThreadId.Should().Be(setterThread.ManagedThreadId);
        }

    }

}