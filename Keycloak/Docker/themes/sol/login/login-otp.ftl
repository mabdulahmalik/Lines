<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('totp') imgRight="signin"; section>
    <#if section="form">
        <@components.cardMain header="${msg('twoFactorAuthentication')}" hasBackToLoginLink=true displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">
                <#if auth?has_content && !usernameHidden?? && auth.showUsername() && !auth.showResetCredentials()>
                    <@components.formField
                        type="text"
                        id="username"
                        name="username"
                        value="${(auth.attemptedUsername!'')}"
                        autocomplete="off"
                        booleanAttributes=["disabled"]
                        labelText="${(!realm.loginWithEmailAllowed)?then(msg('username'), (!realm.registrationEmailAsUsername)?then(msg('usernameOrEmail'), msg('email')))}"
                        hasError=messagesPerField.existsError('username','password')
                        errorMessage="${kcSanitize(messagesPerField.getFirstError('username','password'))?no_esc}"
                    />
                </#if>

                <#if otpLogin.userOtpCredentials?size gt 1>
                    <#list otpLogin.userOtpCredentials as otpCredential>
                        <@components.formField
                            type="radio"
                            id="${otpCredential.id}"
                            name="selectedCredentialId"
                            value="${otpCredential.id}"
                            booleanAttributes=["${(otpCredential.id == otpLogin.selectedCredentialId)?then('checked', '')}"]
                            labelText="${otpCredential.userLabel}"
                        />
                    </#list>
                </#if>

                <@components.formField
                    type="hidden"
                    id="otp"
                    name="otp"
                    autocomplete="off"
                    booleanAttributes=["autofocus"]
                    labelText="${msg('loginOtpOneTime')}"
                    hasError=messagesPerField.existsError('totp')
                    errorMessage="${kcSanitize(messagesPerField.get('totp'))?no_esc}"
                />

                <@components.otpInput inputId="otp" btnId="submit-btn"/>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary" id="submit-btn">
                        ${msg("verify")}
                    </@components.button>
                </div>

                <div class="flex flex-row space-x-5 items-center text-secondary">
                    <div>
                        <img width="40" height="40" src="${url.resourcesPath}/img/smart-phone-01.svg" alt="smart phone">
                    </div>
                    <div>
                        Open the two-factor authenticator app on your device to view your authentication code and verify your identity.
                    </div>
                </div>

<#--                <#if auth?has_content && auth.showUsername() && !auth.showResetCredentials()>-->
<#--                    <hr>-->
<#--                    <div class="${properties.textCenterClass!}">-->
<#--                        <@components.link href="${url.loginRestartFlowUrl}">-->
<#--                            ${msg("restartLoginTooltip")}-->
<#--                        </@components.link>-->
<#--                    </div>-->
<#--                </#if>-->
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>