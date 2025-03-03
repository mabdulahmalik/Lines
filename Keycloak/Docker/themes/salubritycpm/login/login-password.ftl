<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('password'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('continueWithYourEmail')}" hasBackToLoginLink=true>
            <form onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post">
                <#if !usernameHidden?? && (login.username)??>
                    <@components.formField
                    type="username"
                    id="username"
                    name="username"
                    value="${(login.username!)}"
                    booleanAttributes=["disabled"]
                    labelText="${(!realm.loginWithEmailAllowed)?then(msg('username'), (!realm.registrationEmailAsUsername)?then(msg('usernameOrEmail'), msg('email')))}"
                    hasError=messagesPerField.existsError('username')
                    errorMessage="${kcSanitize(messagesPerField.getFirstError('username'))?no_esc}"
                    />
                </#if>
                <@components.formField
                    type="password"
                    id="password"
                    name="password"
                    booleanAttributes=["autofocus"]
                    labelText="${msg('password')}"
                    hasError=messagesPerField.existsError('password')
                    errorMessage="${kcSanitize(messagesPerField.getFirstError('password'))?no_esc}"
                />

                <div class="${properties.loginRememberMeForgotPasswordWrapperClass!}">
                    <#if realm.resetPasswordAllowed>
                        <@components.link href="${url.loginResetCredentialsUrl}">
                            ${msg("doForgotPassword")}
                        </@components.link>
                    </#if>
                </div>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("continue")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
