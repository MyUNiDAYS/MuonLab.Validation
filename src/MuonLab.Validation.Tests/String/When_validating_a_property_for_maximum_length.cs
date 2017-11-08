using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace MuonLab.Validation.Tests.String
{
	public class When_validating_a_property_for_maximum_length
	{
		[Fact]
		public async Task ensure_nulls_pass_validation()
		{
			var testClass = new TestClass(null);

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.IsValid.ShouldBeTrue();
		}

		[Fact]
		public async Task ensure_strings_that_are_too_long_fail_validation()
		{
			var testClass = new TestClass("123456");

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.Violations.First().Error.Key.ShouldEqual("MaxLength");
			validationReport.Violations.First().Error.Replacements["arg0"].ToString().ShouldEqual("5");
		}

		[Fact]
		public async Task ensure_strings_that_are_the_maximum_length_pass_validation()
		{
			var testClass = new TestClass("12345");

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.IsValid.ShouldBeTrue();
		}

		[Fact]
		public async Task ensure_strings_that_are_shorter_than_the_maximum_length_pass_validation()
		{
			var testClass = new TestClass("1234");

			var validator = new TestClassValidator();
			var validationReport = await validator.Validate(testClass);

			validationReport.IsValid.ShouldBeTrue();
		}

		private class TestClass
		{
			public string value { get; set; }

			public TestClass(string value)
			{
				this.value = value;
			}
		}

		private class TestClassValidator : Validator<TestClass>
		{
			protected override void Rules()
			{
				Ensure(x => x.value.HasMaximumLength(5));
			}
		}
	}
}