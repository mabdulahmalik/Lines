<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${client.name?has_content?then(msg("oauthGrantTitle",advancedMsg(client.name)), msg("oauthGrantTitle",client.clientId))}">
            <#if client.attributes.logoUri??>
                <img src="${client.attributes.logoUri}" alt="logo"/>
            </#if>
            <div>
                <h3>
                    ${msg("oauthGrantRequest")}
                </h3>
                <ul>
                    <#if oauth.clientScopesRequested??>
                        <#list oauth.clientScopesRequested as clientScope>
                            <li>
                                <span>
                                    <#if !clientScope.dynamicScopeParameter??>
                                        ${advancedMsg(clientScope.consentScreenText)}
                                    <#else>
                                        ${advancedMsg(clientScope.consentScreenText)}: <b>${clientScope.dynamicScopeParameter}</b>
                                    </#if>
                                </span>
                            </li>
                        </#list>
                    </#if>
                </ul>
                <#if client.attributes.policyUri?? || client.attributes.tosUri??>
                    <h3>
                        <#if client.name?has_content>
                            ${msg("oauthGrantInformation",advancedMsg(client.name))}
                        <#else>
                            ${msg("oauthGrantInformation",client.clientId)}
                        </#if>
                        <#if client.attributes.tosUri??>
                            ${msg("oauthGrantReview")}
                            <@components.link href="${client.attributes.tosUri}" target="_blank">
                                ${msg("oauthGrantTos")}
                            </@components.link>
                        </#if>
                        <#if client.attributes.policyUri??>
                            ${msg("oauthGrantReview")}
                            <@components.link href="${client.attributes.policyUri}" target="_blank">
                                ${msg("oauthGrantPolicy")}
                            </@components.link>
                        </#if>
                    </h3>
                </#if>

                <form action="${url.oauthAction}" method="post">
                    <@components.formField
                        type="hidden"
                        id="code"
                        name="code"
                        value="${oauth.code}"
                    />

                    <div class="${properties.btnWrapperClass!}">
                        <@components.button type="submit" name="accept" value="${msg("doYes")}">
                            ${msg("doYes")}
                        </@components.button>
                        <@components.button type="submit" name="cancel" value="${msg("doNo")}">
                            ${msg("doNo")}
                        </@components.button>
                    </div>
                </form>
            </div>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
