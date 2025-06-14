using System.Diagnostics;
using FC.BuildingBlocks.Infrastructure.Messaging;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.Tests
{
    public class CorrelationContextTests
    {
        [Fact(DisplayName = "Set deve armazenar corretamente o CorrelationId")]
        public void Set_DeveArmazenarCorrelationId()
        {
            // Arrange
            var context = new CorrelationContext();
            var expected = "123456";

            // Act
            context.Set(expected);

            // Assert
            Assert.Equal(expected, context.CorrelationId);
        }

        [Fact(DisplayName = "CorrelationId deve retornar TraceId da Activity atual se não houver valor definido")]
        public void CorrelationId_SemValorDefinido_DeveRetornarTraceIdDaActivity()
        {
            // Arrange
            var activity = new Activity("TestActivity");
            activity.Start();
            var expected = activity.TraceId.ToString();

            var context = new CorrelationContext();

            // Act
            var actual = context.CorrelationId;

            activity.Stop();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "CorrelationId deve retornar tag CorrelationId se estiver presente na Activity")]
        public void CorrelationId_TagDefinidaNaActivity_DeveRetornarValorDaTag()
        {
            // Arrange
            var activity = new Activity("TestActivity");
            activity.SetTag("CorrelationId", "tag-abc");
            activity.Start();

            var context = new CorrelationContext();

            // Act
            var actual = context.CorrelationId;

            activity.Stop();

            // Assert
            Assert.Equal("tag-abc", actual);
        }

        [Fact(DisplayName = "CorrelationId deve retornar 'N/A' se nenhum valor estiver disponível")]
        public void CorrelationId_SemNadaDisponivel_DeveRetornarNA()
        {
            // Arrange
            var context = new CorrelationContext();

            // Garante que não há Activity atual
            Activity.Current = null;

            // Act
            var result = context.CorrelationId;

            // Assert
            Assert.Equal("N/A", result);
        }
    }
}
