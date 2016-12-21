package jp.gr.java_conf.schkit.enums;

public enum ExportFormats {
	None(0, ""),
	Log(1 << 0, "Log"),
	Tab(1 << 1, "Tab"),
	Xml(1 << 2, "Xml"),
	Json(1 << 3, "Json"),
	Html(1 << 4, "Html");

	private final int id;
	private final String name;
	private ExportFormats(int id, String name)
	{
		this.id = id;
		this.name= name;
	}
	public int getId()
	{
		return id;
	}
	public String getName()
	{
		return name;
	}
	public static ExportFormats fromInt(int id)
	{
		for(ExportFormats format : values())
		{
			if(format.getId() == id)
				return format;
		}
		return None;
	}
	public static ExportFormats fromName(String name)
	{
		for(ExportFormats format : values())
		{
			if(format.getName() == name)
				return format;
		}
		return None;
	}
}
