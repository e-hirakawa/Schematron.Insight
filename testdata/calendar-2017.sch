<schema xmlns="http://purl.oclc.org/dsdl/schematron" queryBinding="xslt2">
    <pattern id="test">
        <rule context="day">
            <report test="@day-of-week='Sunday' and parent::week[@number='1']">Holidays at the beginning of the month</report>
            <report test="@day-of-week='Friday' and @number='13'" role="Warning">Friday the 13th...</report>
            <report test="@number='25' and ancestor::month[@value='12']">Merry Christmas</report>
        </rule>
    </pattern>
</schema>
