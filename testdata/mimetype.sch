<schema xmlns="http://purl.oclc.org/dsdl/schematron" queryBinding="xslt2">
    <title>Schema for MimeType</title>
    <pattern id="task1" name="Exists Check of Element ">
        <rule context="item">
            <assert test="name">don't contain name.</assert>
            <assert test="mimetype">don't contain mimetype.</assert>
            <assert test="extensions">extensions is undefined.</assert>
        </rule>
    </pattern>
    <pattern is-a="check-empty-node" id="check-empty-name">
        <param name="node" value="item/name" />
    </pattern>
    <pattern is-a="check-empty-node" id="check-empty-mimetype">
        <param name="node" value="item/mimetype" />
    </pattern>
    <pattern id="check-empty-extensions">
        <rule context="item/extensions">
            <assert test="normalize-space(@value)">@value is empty or undefined.</assert>
        </rule>
    </pattern>
    <pattern id="check-duplication">
        <rule context="item/name">
            <report test="count(../name)&gt;1">'<value-of select="local-name()" />' is duplication.</report>
        </rule>
        <rule context="item/mimetype">
            <report test="count(../mimetype)&gt;1">'<value-of select="local-name()" />' is duplication.</report>
        </rule>
    </pattern>
    <!--abstract patterns-->
    <pattern abstract="true" id="check-empty-node">
        <rule context="$node">
            <assert test="normalize-space(.)">'<value-of select="local-name()" />' is empty.</assert>
        </rule>
    </pattern>
</schema>