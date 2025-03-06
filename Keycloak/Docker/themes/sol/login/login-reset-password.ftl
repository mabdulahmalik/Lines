<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('username') imgRight="forgot-password"; section>
    <#if section = "form">
        <@components.cardMain header="${msg('emailForgotTitle')}" hasBackToLoginLink=true displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">
                <@components.formField
                    type="text"
                    id="username"
                    name="username"
                    value="${(auth.attemptedUsername!'')}"
                    autocomplete="off"
                    booleanAttributes=["autofocus"]
                    labelText="${(!realm.loginWithEmailAllowed)?then(msg('username'), (!realm.registrationEmailAsUsername)?then(msg('usernameOrEmail'), msg('email')))}"
                    hasError=messagesPerField.existsError('username')
                    errorMessage="${kcSanitize(messagesPerField.get('username'))?no_esc}"
                />

                <#if realm.duplicateEmailsAllowed>
                    ${msg("emailInstructionUsername")}
                <#else>
                    ${msg("emailInstruction")}
                </#if>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("resetPassword")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
