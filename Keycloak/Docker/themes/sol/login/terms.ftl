<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${msg('termsTitle')}" displayMessage=displayMessage>
            <div>
                ${kcSanitize(msg("termsText"))?no_esc}
            </div>
            <form action="${url.loginAction}" method="POST">
                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary" name="accept" value="${msg('doAccept')}">
                        ${msg("doAccept")}
                    </@components.button>
                    <@components.button type="submit" variant="default" name="cancel" value="${msg('doDecline')}">
                        ${msg("doDecline")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
