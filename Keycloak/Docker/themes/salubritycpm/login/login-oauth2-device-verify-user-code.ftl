<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('device-user-code'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('oauth2DeviceVerificationTitle')}">
            <form action="${url.oauth2DeviceVerificationAction}" method="post">
                <@components.formField
                    type="text"
                    id="device-user-code"
                    name="device-user-code"
                    value="${(login.username!'')}"
                    autocomplete="off"
                    booleanAttributes=["autofocus"]
                    labelText="${msg("verifyOAuth2DeviceUserCode")}"
                    hasError=messagesPerField.existsError('device-user-code')
                    errorMessage="${kcSanitize(messagesPerField.existsError('device-user-code'))?no_esc}"
                />

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary">
                        ${msg("doSubmit")}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>