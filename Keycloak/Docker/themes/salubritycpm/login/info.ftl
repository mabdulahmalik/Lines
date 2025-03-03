<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${messageHeader?has_content?then(messageHeader, messsage.summary)}">
            <p>
                ${message.summary}
                <#if requiredActions??>
                    <#list requiredActions>:
                        <b>
                            <#items as reqActionItem>
                                ${kcSanitize(msg("requiredAction.${reqActionItem}"))?no_esc}<#sep>,
                            </#items>
                        </b>
                    </#list>
                <#else>

                </#if>
            </p>
            <#if skipLink??>
            <#else>
                <#if pageRedirectUri?has_content>
                    <p>
                        <@components.link href="${pageRedirectUri}">
                            ${kcSanitize(msg("backToApplication"))?no_esc}
                        </@components.link>
                    </p>
                <#elseif actionUri?has_content>
                    <p>
                        <@components.link href="${actionUri}">
                            ${kcSanitize(msg("proceedWithAction"))?no_esc}
                        </@components.link>
                    </p>
                <#elseif (client.baseUrl)?has_content>
                    <p>
                        <@components.link href="${client.baseUrl}">
                            ${kcSanitize(msg("backToApplication"))?no_esc}
                        </@components.link>
                    </p>
                </#if>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>