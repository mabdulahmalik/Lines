<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout ; section>
    <#if section = "form">
        <@components.cardMain header="${msg('emailVerifyTitle')}">
            <p>${msg("emailVerifyInstruction1",user.email)}</p>
            <p>
                ${msg("emailVerifyInstruction2")}
                <@components.link href="${url.loginAction}">
                    ${msg("doClickHere")}
                </@components.link>
                ${msg("emailVerifyInstruction3")}
            </p>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
