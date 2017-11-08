using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MuonLab.Validation.Tests.IComparable.GreaterThan
{
	
	public class when_validating_a_property_as_greater_than_another
	{
		[Fact]
		public async Task test_1_greater_than_4_returns_false()
		{
			var testClass = new TestClass(1, 4);

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.Violations.First().Error.Key.ShouldEqual("GreaterThan");
			validationReport.Violations.First().Error.Replacements["arg0"].ToString().ShouldEqual("Value2");
		}

		[Fact]
		public async Task test_4_greater_than_1_returns_true()
		{
			var testClass = new TestClass(4, 1);

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.IsValid.ShouldBeTrue();
		}

		[Fact]
		public async Task test_2_greater_than_2_returns_false()
		{
			var testClass = new TestClass(2, 2);

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.Violations.First().Error.Key.ShouldEqual("GreaterThan");
			validationReport.Violations.First().Error.Replacements["arg0"].ToString().ShouldEqual("Value2");
		}

		private class TestClass
		{
			public int Value { get; }
			public int? Value2 { get; }

			public TestClass(int value, int value2)
			{
				this.Value = value;
				this.Value2 = value2;
			}
		}

		private class TestClassValidator : Validator<TestClass>
		{
			protected override void Rules()
			{
				Ensure(x => x.Value.IsGreaterThan(x.Value2));
			}
		}
	}
}