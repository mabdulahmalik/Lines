<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('recoveryCodeInput'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('auth-recovery-code-header')}" displayMessage=displayMessage>
            <form action="${url.loginAction}" method="post">
                <@components.formField
                    type="text"
                    id="recoveryCodeInput"
                    name="recoveryCodeInput"
                    value=""
                    autocomplete="off"
                    booleanAttributes=["autofocus"]
                    labelText="${msg('auth-recovery-code-prompt', recoveryAuthnCodesInputBean.codeNumber?c)}"
                    hasError=messagesPerField.existsError('recoveryCodeInput')
                    errorMessage="${kcSanitize(messagesPerField.get('recoveryCodeInput'))?no_esc}"
                />

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("doLogIn")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>