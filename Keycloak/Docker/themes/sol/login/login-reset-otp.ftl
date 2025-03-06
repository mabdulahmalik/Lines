<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('totp'); section>
    <#if section="form">
        <@components.cardMain header="${msg('doLogIn')}" displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">
                <p>
                    ${msg("otp-reset-description")}
                </p>

                <#list configuredOtpCredentials.userOtpCredentials as otpCredential>
                    <@components.formField
                        type="radio"
                        id="${otpCredential.id}"
                        name="selectedCredentialId"
                        value="${otpCredential.id}"
                        booleanAttributes=["${(otpCredential.id == configuredOtpCredentials.selectedCredentialId)?then('checked', '')}"]
                        labelText="${otpCredential.userLabel}"
                    />
                </#list>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("doSubmit")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
