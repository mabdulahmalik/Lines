<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${kcSanitize(msg('errorTitle'))?no_esc}" displayMessage=displayMessage>
            <p>
                ${kcSanitize(message.summary)?no_esc}
            </p>

            <#if skipLink??>
            <#else>
                <#if client?? && client.baseUrl?has_content>
                    <hr>
                    <div class="${properties.textCenterClass!}">
                        <@components.link href="${client.baseUrl}">
                            ${kcSanitize(msg("backToApplication"))?no_esc}
                        </@components.link>
                    </div>
                </#if>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>