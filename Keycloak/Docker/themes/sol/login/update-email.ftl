<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<#import "lib/commons.ftl" as commons>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('email'); section>
    <#if section = "form">
        <@components.cardMain header=" ${msg('updateEmailTitle')}" displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">

                <@commons.userProfileFormFields/>

                <@commons.logoutOtherSessions/>

                <div class="${properties.btnWrapperClass!}">
                    <#if isAppInitiatedAction??>
                        <@components.button type="submit" variant="primary">
                            ${msg("doSubmit")}
                        </@components.button>
                        <@components.button type="submit" variant="secondary" name="cancel-aia" value="true">
                            ${msg("doCancel")}
                        </@components.button>
                    <#else>
                        <@components.button type="submit" variant="primary">
                            ${msg("doSubmit")}
                        </@components.button>
                    </#if>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
