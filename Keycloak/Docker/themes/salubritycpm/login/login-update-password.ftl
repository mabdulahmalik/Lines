<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('password','password-confirm'); section>
    <#if section = "form">
        <@components.cardMain header="${msg('setupNewPassword')}">

            ${msg("enterYourNewPasswordAndYouAreGoodToGo")}

            <form action="${url.loginAction}" method="post">
                <@components.formField
                    type="hidden"
                    id="username"
                    name="username"
                    value="${username}"
                />

                <@components.formField
                    type="password"
                    id="password-new"
                    name="password-new"
                    autocomplete="new-password"
                    booleanAttributes=["autofocus"]
                    labelText="${msg('passwordNew')}"
                    hasError=messagesPerField.existsError('password')
                    errorMessage="${kcSanitize(messagesPerField.get('password'))?no_esc}"
                />

                <@components.formField
                    type="password"
                    id="password-confirm"
                    name="password-confirm"
                    autocomplete="new-password"
                    labelText="${msg('passwordConfirm')}"
                    hasError=messagesPerField.existsError('password-confirm')
                    errorMessage="${kcSanitize(messagesPerField.get('password-confirm'))?no_esc}"
                />

                <@components.formField
                    type="checkbox"
                    id="logout-sessions"
                    name="logout-sessions"
                    booleanAttributes=["checked"]
                    labelText="${msg('logoutOtherSessions')}"
                />

                <div class="${properties.btnWrapperClass!}">
                    <#if !isAppInitiatedAction??>
                        <@components.button type="submit" variant="primary">
                            ${msg("continue")}
                        </@components.button>
                        <@components.button type="submit" variant="default" name="cancel-aia" value="true">
                            ${msg("doCancel")}
                        </@components.button>
                    <#else>
                        <@components.button type="submit" variant="primary">
                            ${msg("continue")}
                        </@components.button>
                    </#if>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
