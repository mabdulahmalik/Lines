<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('pageExpiredTitle')}">
            <p>
                ${msg("pageExpiredMsg1")}
                <@components.link href="${url.loginRestartFlowUrl}">
                    ${msg("doClickHere")}
                </@components.link>
                <br/>
                ${msg("pageExpiredMsg2")}
                <@components.link href="${url.loginAction}">
                    ${msg("doClickHere")}
                </@components.link>
            </p>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
