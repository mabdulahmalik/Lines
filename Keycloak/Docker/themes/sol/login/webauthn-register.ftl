<#import "template.ftl" as layout>
<#import "lib/components.ftl" as macros>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@macros.cardMain header="${kcSanitize(msg('webauthn-registration-title'))?no_esc}">
            <form id="register" action="${url.loginAction}" method="post">
                <@macros.formField
                    type="hidden"
                    id="clientDataJSON"
                    name="clientDataJSON"
                />
                <@macros.formField
                    type="hidden"
                    id="attestationObject"
                    name="attestationObject"
                />
                <@macros.formField
                    type="hidden"
                    id="publicKeyCredentialId"
                    name="publicKeyCredentialId"
                />
                <@macros.formField
                    type="hidden"
                    id="authenticatorLabel"
                    name="authenticatorLabel"
                />
                <@macros.formField
                    type="hidden"
                    id="transports"
                    name="transports"
                />
                <@macros.formField
                    type="hidden"
                    id="error"
                    name="error"
                />

                <@macros.formField
                    type="checkbox"
                    id="logout-sessions"
                    name="logout-sessions"
                    value="on"
                    booleanAttributes=["checked"]
                    labelText="${msg('logoutOtherSessions')}"
                />
            </form>

            <script type="module">
                import { registerByWebAuthn } from "${url.resourcesPath}/js/webauthnRegister.js";
                const registerButton = document.getElementById('registerWebAuthn');
                registerButton.addEventListener("click", function() {
                    const input = {
                        challenge : '${challenge}',
                        userid : '${userid}',
                        username : '${username}',
                        signatureAlgorithms : [<#list signatureAlgorithms as sigAlg>${sigAlg?c},</#list>],
                        rpEntityName : '${rpEntityName}',
                        rpId : '${rpId}',
                        attestationConveyancePreference : '${attestationConveyancePreference}',
                        authenticatorAttachment : '${authenticatorAttachment}',
                        requireResidentKey : '${requireResidentKey}',
                        userVerificationRequirement : '${userVerificationRequirement}',
                        createTimeout : ${createTimeout},
                        excludeCredentialIds : '${excludeCredentialIds}',
                        initLabel : "${msg("webauthn-registration-init-label")?no_esc}",
                        initLabelPrompt : "${msg("webauthn-registration-init-label-prompt")?no_esc}",
                        errmsg : "${msg("webauthn-unsupported-browser-text")?no_esc}"
                    };
                    registerByWebAuthn(input);
                });
            </script>

            <div class="d-grid">
                <@macros.button type="submit" variant="primary" id="registerWebAuthn">
                    ${msg("doRegisterSecurityKey")}
                </@macros.button>
            </div>

            <#if !isSetRetry?has_content && isAppInitiatedAction?has_content>
                <form action="${url.loginAction}" method="post">
                    <div class="d-grid">
                        <@macros.button type="submit" variant="default" id="cancelWebAuthnAIA" name="cancel-aia" value="true">
                            ${msg("doCancel")}
                        </@macros.button>
                    </div>
                </form>
            </#if>
        </@macros.cardMain>
    </#if>
</@layout.registrationLayout>
