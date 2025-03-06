<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>
<#import "lib/commons.ftl" as commons>

<@layout.registrationLayout displayMessage=messagesPerField.exists('global'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('loginIdpReviewProfileTitle')}" displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">

                <@commons.userProfileFormFields/>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("doSubmit")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>