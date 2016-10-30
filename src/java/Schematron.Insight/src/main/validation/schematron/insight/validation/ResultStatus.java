package schematron.insight.validation;

import java.util.EnumSet;

public enum ResultStatus {

	None{
		@Override
		public String toString() { return "None"; }
		public int toInteger() { return 0; }
	},
	SyntaxError{
		@Override
		public String toString() { return "Syntax Error"; }
		public int toInteger() { return 1 << 0; }
	},
	Assert{
		@Override
		public String toString() { return "Assert"; }
		public int toInteger() { return 1 << 1; }
	},
	Report{
		@Override
		public String toString() { return "Report"; }
		public int toInteger() { return 1 << 2; }
	};
	abstract int toInteger();

	public boolean isContains(EnumSet<ResultStatus> target){
		return target.contains(this);
	}
}
