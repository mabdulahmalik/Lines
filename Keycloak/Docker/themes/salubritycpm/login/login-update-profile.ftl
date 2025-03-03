<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>
<#import "lib/commons.ftl" as commons>

<@layout.registrationLayout displayMessage=messagesPerField.exists('global'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('loginProfileTitle')}">
            <form action="${url.loginAction}" method="post">

                <@commons.userProfileFormFields/>

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
