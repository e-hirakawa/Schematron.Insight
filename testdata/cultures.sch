<schema xmlns="http://purl.oclc.org/dsdl/schematron" queryBinding="xslt2">
    <pattern id="test">
        <rule context="culture">
            <let name="disp" value="name[@lang='en']" />
            <report test="@written-from='RtoL'">
                <value-of select="$disp" /> is written from Right to Left.</report>
        </rule>
    </pattern>
</schema>