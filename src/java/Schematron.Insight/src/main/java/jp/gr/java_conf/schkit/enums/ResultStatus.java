package jp.gr.java_conf.schkit.enums;

public enum ResultStatus {
	None(0, "None"),
	SyntaxError(1 << 0, "Syntax Error"),
	Assert(1 << 1, "Assert"),
	Report(1 << 2, "Report");

	private final int id;
	private final String name;

	private ResultStatus(int id, String name) {
		this.id = id;
		this.name = name;
	}
	public int getId()
	{
		return id;
	}
	public String getName()
	{
		return name;
	}
	public static ResultStatus fromInt(int id)
	{
		for(ResultStatus format : values())
		{
			if(format.getId() == id)
				return format;
		}
		return None;
	}
	public static ResultStatus fromName(String name)
	{
		for(ResultStatus format : values())
		{
			if(format.getName() == name)
				return format;
		}
		return None;
	}
}
