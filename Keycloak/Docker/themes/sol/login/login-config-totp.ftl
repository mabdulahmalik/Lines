<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=!messagesPerField.existsError('totp','userLabel') imgRight="create-account"; section>
    <#if section = "form">
        <@components.cardMain header="${msg('loginTotpTitle')}" displayMessage=displayMessage>
            <ol>
                <li>
                    ${msg("loginTotpStep1")}
                    <ul>
                        <#list totp.supportedApplications as app>
                            <li>${msg(app)}</li>
                        </#list>
                    </ul>
                </li>

                <#if mode?? && mode = "manual">
                    <li>
                        ${msg("loginTotpManualStep2")}
                        <br>
                        ${totp.totpSecretEncoded}
                        <br>
                        <@components.link href="${totp.qrUrl}">
                            ${msg("loginTotpScanBarcode")}
                        </@components.link>
                    </li>
                    <li>
                        ${msg("loginTotpManualStep3")}
                        <ul>
                            <li>${msg("loginTotpType")}: ${(totp.policy.type == "totp")?then(msg("loginTotp.totp"), msg("loginTotp.hotp"))}</li>
                            <li>${msg("loginTotpAlgorithm")}: ${totp.policy.getAlgorithmKey()}</li>
                            <li>${msg("loginTotpDigits")}: ${totp.policy.digits}</li>
                            <#if totp.policy.type = "totp">
                                <li>${msg("loginTotpInterval")}: ${totp.policy.period}</li>
                            <#elseif totp.policy.type = "hotp">
                                <li>${msg("loginTotpCounter")}: ${totp.policy.initialCounter}</li>
                            </#if>
                        </ul>
                    </li>
                <#else>
                    <li>
                        ${msg("loginTotpStep2")}
                        <br>
                        <img src="data:image/png;base64, ${totp.totpSecretQrCode}" alt="Figure: Barcode">
                        <br>
                        <@components.link href="${totp.manualUrl}">
                            ${msg("loginTotpUnableToScan")}
                        </@components.link>
                    </li>
                </#if>
                <li>
                    ${msg("loginTotpStep3")}
                    <br>
                    ${msg("loginTotpStep3DeviceName")}
                </li>
            </ol>

            <form action="${url.loginAction}" method="post">
                <@components.formField
                type="hidden"
                id="totp"
                name="totp"
                autocomplete="off"
                labelText="${msg('authenticatorCode')} *"
                hasError=messagesPerField.existsError('totp')
                errorMessage="${kcSanitize(messagesPerField.get('totp'))?no_esc}"
                />

                <@components.otpInput inputId="totp" btnId="submit-btn"/>

                <@components.formField
                type="hidden"
                id="totpSecret"
                name="totpSecret"
                value="${totp.totpSecret}"
                />

                <#if mode??>
                    <@components.formField
                    type="hidden"
                    id="mode"
                    name="mode"
                    value="${mode}"
                    />
                </#if>

                <@components.formField
                type="text"
                id="userLabel"
                name="userLabel"
                autocomplete="off"
                labelText="${msg('loginTotpDeviceName')} ${(totp.otpCredentials?size gte 1)?then('*', '')}"
                hasError=messagesPerField.existsError('userLabel')
                errorMessage="${kcSanitize(messagesPerField.get('userLabel'))?no_esc}"
                />

                <@components.formField
                type="checkbox"
                id="logout-sessions"
                name="logout-sessions"
                booleanAttributes=["checked"]
                labelText="${msg('logoutOtherSessions')}"
                />

                <div class="${properties.btnWrapperClass!}">
                    <#if isAppInitiatedAction??>
                        <@components.button type="submit" variant="primary" id="submit-btn">
                            ${msg("doSubmit")}
                        </@components.button>
                        <@components.button type="submit" variant="secondary" name="cancel-aia" value="true">
                            ${msg("doCancel")}
                        </@components.button>
                    <#else>
                        <@components.button type="submit" variant="primary" id="submit-btn">
                            ${msg("doSubmit")}
                        </@components.button>
                    </#if>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
