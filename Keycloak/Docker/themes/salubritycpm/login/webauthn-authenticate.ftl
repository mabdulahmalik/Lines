<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout title=title; section>
    <#if section = "form">
        <@components.cardMain header=" ${kcSanitize(msg('webauthn-login-title'))?no_esc}">
            <form id="webauth" action="${url.loginAction}" method="post">
                <@components.formField
                    type="hidden"
                    id="clientDataJSON"
                    name="clientDataJSON"
                />
                <@components.formField
                    type="hidden"
                    id="authenticatorData"
                    name="authenticatorData"
                />
                <@components.formField
                    type="hidden"
                    id="signature"
                    name="signature"
                />
                <@components.formField
                    type="hidden"
                    id="credentialId"
                    name="credentialId"
                />
                <@components.formField
                    type="hidden"
                    id="userHandle"
                    name="userHandle"
                />
                <@components.formField
                    type="hidden"
                    id="error"
                    name="error"
                />
            </form>

            <#if authenticators??>
                <form id="authn_select">
                    <#list authenticators.authenticators as authenticator>
                        <@components.formField
                            type="hidden"
                            id="${authenticator.credentialId}"
                            name="authn_use_chk"
                            value="${authenticator.credentialId}"
                        />
                    </#list>
                </form>

                <#if shouldDisplayAuthenticators?? && shouldDisplayAuthenticators>
                    <#if authenticators.authenticators?size gt 1>
                        <p>
                            ${kcSanitize(msg("webauthn-available-authenticators"))?no_esc}
                        </p>
                    </#if>

                    <ul>
                        <#list authenticators.authenticators as authenticator>
                            <li>
                                ${kcSanitize(msg('${authenticator.label}'))?no_esc}

                                <#if authenticator.transports?? && authenticator.transports.displayNameProperties?has_content>
                                    [
                                        <#list authenticator.transports.displayNameProperties as nameProperty>
                                            <span>${kcSanitize(msg('${nameProperty!}'))?no_esc}</span>
                                            <#if nameProperty?has_next>
                                                <span>, </span>
                                            </#if>
                                        </#list>
                                    ]
                                </#if>

                                [
                                    ${kcSanitize(msg('webauthn-createdAt-label'))?no_esc}
                                    ${kcSanitize(authenticator.createdAt)?no_esc}
                                ]
                            </li>
                        </#list>
                    </ul>
                </#if>
            </#if>

            <div class="${properties.btnWrapperClass!}">
                <@components.button type="button" variant="primary" onclick="webAuthnAuthenticate()" autofocus="autofocus">
                    ${kcSanitize(msg("webauthn-doAuthenticate"))}
                </@components.button>
            </div>
        </@components.cardMain>


        <script type="text/javascript" src="${url.resourcesCommonPath}/node_modules/jquery/dist/jquery.min.js"></script>
        <script type="text/javascript" src="${url.resourcesPath}/js/base64url.js"></script>
        <script type="text/javascript">
            function webAuthnAuthenticate() {
                let isUserIdentified = ${isUserIdentified};
                if (!isUserIdentified) {
                    doAuthenticate([]);
                    return;
                }
                checkAllowCredentials();
            }

            function checkAllowCredentials() {
                let allowCredentials = [];
                let authn_use = document.forms['authn_select'].authn_use_chk;

                if (authn_use !== undefined) {
                    if (authn_use.length === undefined) {
                        allowCredentials.push({
                            id: base64url.decode(authn_use.value, {loose: true}),
                            type: 'public-key',
                        });
                    } else {
                        for (let i = 0; i < authn_use.length; i++) {
                            allowCredentials.push({
                                id: base64url.decode(authn_use[i].value, {loose: true}),
                                type: 'public-key',
                            });
                        }
                    }
                }
                doAuthenticate(allowCredentials);
            }


            function doAuthenticate(allowCredentials) {

            // Check if WebAuthn is supported by this browser
            if (!window.PublicKeyCredential) {
                $("#error").val("${msg("webauthn-unsupported-browser-text")?no_esc}");
                $("#webauth").submit();
                return;
            }

            let challenge = "${challenge}";
            let userVerification = "${userVerification}";
            let rpId = "${rpId}";
            let publicKey = {
                rpId : rpId,
                challenge: base64url.decode(challenge, { loose: true })
            };

            let createTimeout = ${createTimeout};
            if (createTimeout !== 0) publicKey.timeout = createTimeout * 1000;

            if (allowCredentials.length) {
                publicKey.allowCredentials = allowCredentials;
            }

            if (userVerification !== 'not specified') publicKey.userVerification = userVerification;

            navigator.credentials.get({publicKey})
                .then((result) => {
                    window.result = result;

                    let clientDataJSON = result.response.clientDataJSON;
                    let authenticatorData = result.response.authenticatorData;
                    let signature = result.response.signature;

                    $("#clientDataJSON").val(base64url.encode(new Uint8Array(clientDataJSON), { pad: false }));
                    $("#authenticatorData").val(base64url.encode(new Uint8Array(authenticatorData), { pad: false }));
                    $("#signature").val(base64url.encode(new Uint8Array(signature), { pad: false }));
                    $("#credentialId").val(result.id);
                    if(result.response.userHandle) {
                        $("#userHandle").val(base64url.encode(new Uint8Array(result.response.userHandle), { pad: false }));
                    }
                    $("#webauth").submit();
                })
                .catch((err) => {
                    $("#error").val(err);
                    $("#webauth").submit();
                });
            }
        </script>
    </#if>
</@layout.registrationLayout>
