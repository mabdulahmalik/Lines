<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>


<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${kcSanitize(msg('passkey-login-title'))?no_esc}" displayMessage=displayMessage>
            <form id="webauth" action="${url.loginAction}" method="post">
                <input type="hidden" id="clientDataJSON" name="clientDataJSON"/>
                <input type="hidden" id="authenticatorData" name="authenticatorData"/>
                <input type="hidden" id="signature" name="signature"/>
                <input type="hidden" id="credentialId" name="credentialId"/>
                <input type="hidden" id="userHandle" name="userHandle"/>
                <input type="hidden" id="error" name="error"/>
            </form>

            <#if authenticators??>
                <form id="authn_select">
                    <#list authenticators.authenticators as authenticator>
                        <input type="hidden" name="authn_use_chk" value="${authenticator.credentialId}"/>
                    </#list>
                </form>

                <#if shouldDisplayAuthenticators?? && shouldDisplayAuthenticators>
                    <#if authenticators.authenticators?size gt 1>
                        <p class="${properties.kcSelectAuthListItemTitle!}">${kcSanitize(msg("passkey-available-authenticators"))?no_esc}</p>
                    </#if>

                    <div>
                        <#list authenticators.authenticators as authenticator>
                            <div id="kc-webauthn-authenticator-item-${authenticator?index}">
                                <div class="${properties.kcSelectAuthListItemIconClass!}">
                                    <i class="${(properties['${authenticator.transports.iconClass}'])!'${properties.kcWebAuthnDefaultIcon!}'} ${properties.kcSelectAuthListItemIconPropertyClass!}"></i>
                                </div>
                                <div class="${properties.kcSelectAuthListItemBodyClass!}">
                                    <div id="kc-webauthn-authenticator-label-${authenticator?index}"
                                         class="${properties.kcSelectAuthListItemHeadingClass!}">
                                        ${kcSanitize(msg('${authenticator.label}'))?no_esc}
                                    </div>

                                    <#if authenticator.transports?? && authenticator.transports.displayNameProperties?has_content>
                                        <div id="kc-webauthn-authenticator-transport-${authenticator?index}"
                                            class="${properties.kcSelectAuthListItemDescriptionClass!}">
                                            <#list authenticator.transports.displayNameProperties as nameProperty>
                                                <span>${kcSanitize(msg('${nameProperty!}'))?no_esc}</span>
                                                <#if nameProperty?has_next>
                                                    <span>, </span>
                                                </#if>
                                            </#list>
                                        </div>
                                    </#if>

                                    <div class="${properties.kcSelectAuthListItemDescriptionClass!}">
                                        <span id="kc-webauthn-authenticator-createdlabel-${authenticator?index}">
                                            ${kcSanitize(msg('passkey-createdAt-label'))?no_esc}
                                        </span>
                                        <span id="kc-webauthn-authenticator-created-${authenticator?index}">
                                            ${kcSanitize(authenticator.createdAt)?no_esc}
                                        </span>
                                    </div>
                                </div>
                                <div class="${properties.kcSelectAuthListItemFillClass!}"></div>
                            </div>
                        </#list>
                    </div>
                </#if>
            </#if>

            <#if realm.password>
                <form id="kc-form-login" onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post" style="display:none">
                    <#if !usernameHidden??>
                        <@components.formField
                            type="text"
                            id="username"
                            name="username"
                            value="${(login.username!'')}"
                            autocomplete="username webauthn"
                            booleanAttributes=["autofocus"]
                            labelText="${msg('passkey-autofill-select')}"
                            hasError=messagesPerField.existsError('username')
                            errorMessage="${kcSanitize(messagesPerField.get('username'))?no_esc}"
                        />
                    </#if>
                </form>
            </#if>

            <div id="kc-form-passkey-button" class="${properties.btnWrapperClass!}" style="display: none;">
                <@components.button type="submit" variant="primary" id="authenticateWebAuthnButton">
                    ${kcSanitize(msg("passkey-doAuthenticate"))}
                </@components.button>
            </div>


            <script type="module">
            import { authenticateByWebAuthn } from "${url.resourcesPath}/js/webauthnAuthenticate.js";
            import { initAuthenticate } from "${url.resourcesPath}/js/passkeysConditionalAuth.js";

            const authButton = document.getElementById('authenticateWebAuthnButton');
            const input = {
                isUserIdentified : ${isUserIdentified},
                challenge : '${challenge}',
                userVerification : '${userVerification}',
                rpId : '${rpId}',
                createTimeout : ${createTimeout},
                errmsg : "${msg("webauthn-unsupported-browser-text")?no_esc}"
            };
            authButton.addEventListener("click", () => {
                authenticateByWebAuthn(input);
            });

            const args = {
                isUserIdentified : ${isUserIdentified},
                challenge : '${challenge}',
                userVerification : '${userVerification}',
                rpId : '${rpId}',
                createTimeout : ${createTimeout},
                errmsg : "${msg("passkey-unsupported-browser-text")?no_esc}"
            };

            document.addEventListener("DOMContentLoaded", (event) => initAuthenticate(args));
        </script>
        </@components.cardMain>
    </#if>

</@layout.registrationLayout>
