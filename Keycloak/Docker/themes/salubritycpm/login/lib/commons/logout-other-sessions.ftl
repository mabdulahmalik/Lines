<#import "../components.ftl" as components>

<#macro logoutOtherSessions>
    <@components.formField
        type="checkbox"
        id="logout-sessions"
        name="logout-sessions"
        value="on"
        booleanAttributes=["checked"]
        labelText="${msg('logoutOtherSessions')}"
    />
</#macro>
