<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('emailLinkIdpTitle', idpDisplayName)}">
            <ul>
                <li>
                    ${msg("emailLinkIdp1", idpDisplayName, brokerContext.username, realm.displayName)}
                </li>
                <li>
                    ${msg("emailLinkIdp2")}
                    <@components.link href="${url.loginAction}">
                        ${msg("doClickHere")}
                    </@components.link>
                    ${msg("emailLinkIdp3")}
                </li>
                <li>
                    ${msg("emailLinkIdp4")}
                    <@components.link href="${url.loginAction}">
                        ${msg("doClickHere")}
                    </@components.link>
                    ${msg("emailLinkIdp5")}
                </li>
            </ul>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>