using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MuonLab.Validation.Tests
{
	
	public class when_validating_with_a_chained_conditional_rule
	{
		[Fact]
		public async Task when_a_condition_is_false_the_validation_rule_should_not_be_run_and_the_violation_should_appear()
		{
			var testClass = new TestClass(2, 2);

			var validator = new ConditionalValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.Violations.First().Error.Key.ShouldEqual("EqualTo");
			validationReport.Violations.First().Error.Replacements["arg0"].ToString().ShouldEqual("1");

			validationReport.Violations.Count().ShouldEqual(1);
		}

		[Fact]
		public async Task when_a_condition_is_true_the_validation_rule_should_be_run()
		{
			var testClass = new TestClass(1, 2);

			var validator = new ConditionalValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.Violations.First().Error.Key.ShouldEqual("EqualTo");
			validationReport.Violations.First().Error.Replacements["arg0"].ToString().ShouldEqual("3");
			validationReport.Violations.Count().ShouldEqual(1);
		}

		private class TestClass
		{
			public int Value { get; set; }
			public int Value2 { get; set; }

			public TestClass(int value, int value2)
			{
				this.Value = value;
				this.Value2 = value2;
			}
		}

		private class ConditionalValidator : Validator<TestClass>
		{
			protected override void Rules()
			{
				Ensure(x => x.Value.IsEqualTo(1)).And(() => Ensure(x => x.Value2.IsEqualTo(3)));
			}
		}
	}
}