<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('recovery-code-config-header')}">
            <!-- warning -->
            <@components.alertWarning>
                <h4>
                    ${msg("recovery-code-config-warning-title")}
                </h4>
                <p>
                    ${msg("recovery-code-config-warning-message")}
                </p>
            </@components.alertWarning>

            <ol id="recovery-codes-list">
                <#list recoveryAuthnCodesConfigBean.generatedRecoveryAuthnCodesList as code>
                    <li>
                        <span>${code?counter}:</span> ${code[0..3]}-${code[4..7]}-${code[8..]}
                    </li>
                </#list>
            </ol>

            <!-- actions -->
            <div class="${properties.btnWrapperClass!}">
                <@components.button id="printRecoveryCodes">
                    ${msg("recovery-codes-print")}
                </@components.button>
                <@components.button id="downloadRecoveryCodes">
                    ${msg("recovery-codes-download")}
                </@components.button>
                <@components.button id="copyRecoveryCodes">
                    ${msg("recovery-codes-copy")}
                </@components.button>
            </div>

            <!-- confirmation checkbox -->
            <@components.formField
                type="checkbox"
                id="recoveryCodesConfirmationCheck"
                name="recoveryCodesConfirmationCheck"
                labelText="${msg('recovery-codes-confirmation-message')}"
                onchange="document.querySelectorAll('button[type=submit], button[variant=primary]').forEach(btn => btn.toggleAttribute('disabled'))"
            />

            <form action="${url.loginAction}" method="post">
                <@components.formField
                    type="hidden"
                    id="generatedRecoveryAuthnCodes"
                    name="generatedRecoveryAuthnCodes"
                    value="${recoveryAuthnCodesConfigBean.generatedRecoveryAuthnCodesAsString}"
                />
                <@components.formField
                    type="hidden"
                    id="generatedAt"
                    name="generatedAt"
                    value="${recoveryAuthnCodesConfigBean.generatedAt?c}"
                />
                <@components.formField
                    type="hidden"
                    id="userLabel"
                    name="userLabel"
                    value="${msg('recovery-codes-label-default')}"
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
                        <@components.button type="submit" variant="primary" disabled=true>
                            ${msg("recovery-codes-action-complete")}
                        </@components.button>
                        <@components.button type="submit" variant="secondary" name="cancel-aia" value="true">
                            ${msg("recovery-codes-action-cancel")}
                        </@components.button>
                    <#else>
                        <@components.button type="submit" variant="primary" disabled=true>
                            ${msg("recovery-codes-action-complete")}
                        </@components.button>
                    </#if>
                </div>
            </form>

            <script>
                /* copy recovery codes  */
                function copyRecoveryCodes() {
                    var tmpTextarea = document.createElement("textarea");
                    var codes = document.getElementById("recovery-codes-list").getElementsByTagName("li");
                    for (i = 0; i < codes.length; i++) {
                        tmpTextarea.value = tmpTextarea.value + codes[i].innerText + "\n";
                    }
                    document.body.appendChild(tmpTextarea);
                    tmpTextarea.select();
                    document.execCommand("copy");
                    document.body.removeChild(tmpTextarea);
                }

                var copyButton = document.getElementById("copyRecoveryCodes");
                copyButton && copyButton.addEventListener("click", function () {
                    copyRecoveryCodes();
                });

                /* download recovery codes  */
                function formatCurrentDateTime() {
                    var dt = new Date();
                    var options = {
                        month: 'long',
                        day: 'numeric',
                        year: 'numeric',
                        hour: 'numeric',
                        minute: 'numeric',
                        timeZoneName: 'short'
                    };

                    return dt.toLocaleString('en-US', options);
                }

                function parseRecoveryCodeList() {
                    var recoveryCodes = document.querySelectorAll("#recovery-codes-list li");
                    var recoveryCodeList = "";

                    for (var i = 0; i < recoveryCodes.length; i++) {
                        var recoveryCodeLiElement = recoveryCodes[i].innerText;
                        recoveryCodeList += recoveryCodeLiElement + "\r\n";
                    }

                    return recoveryCodeList;
                }

                function buildDownloadContent() {
                    var recoveryCodeList = parseRecoveryCodeList();
                    var dt = new Date();
                    var options = {
                        month: 'long',
                        day: 'numeric',
                        year: 'numeric',
                        hour: 'numeric',
                        minute: 'numeric',
                        timeZoneName: 'short'
                    };

                    return fileBodyContent =
                        "${msg("recovery-codes-download-file-header")}\n\n" +
                        recoveryCodeList + "\n" +
                        "${msg("recovery-codes-download-file-description")}\n\n" +
                        "${msg("recovery-codes-download-file-date")} " + formatCurrentDateTime();
                }

                function setUpDownloadLinkAndDownload(filename, text) {
                    var el = document.createElement('a');
                    el.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
                    el.setAttribute('download', filename);
                    el.style.display = 'none';
                    document.body.appendChild(el);
                    el.click();
                    document.body.removeChild(el);
                }

                function downloadRecoveryCodes() {
                    setUpDownloadLinkAndDownload('download-recovery-codes.txt', buildDownloadContent());
                }

                var downloadButton = document.getElementById("downloadRecoveryCodes");
                downloadButton && downloadButton.addEventListener("click", downloadRecoveryCodes);

                /* print recovery codes */
                function buildPrintContent() {
                    var recoveryCodeListHTML = document.getElementById('recovery-codes-list').innerHTML;
                    var styles =
                        `@page { size: auto;  margin-top: 0; }
                    body { width: 480px; }
                    div { list-style-type: none; font-family: monospace }
                    p:first-of-type { margin-top: 48px }`

                    return printFileContent =
                        "<html><style>" + styles + "</style><body>" +
                        "<title>download-recovery-codes</title>" +
                        "<p>${msg("recovery-codes-download-file-header")}</p>" +
                        "<div>" + recoveryCodeListHTML + "</div>" +
                        "<p>${msg("recovery-codes-download-file-description")}</p>" +
                        "<p>${msg("recovery-codes-download-file-date")} " + formatCurrentDateTime() + "</p>" +
                        "</body></html>";
                }

                function printRecoveryCodes() {
                    var w = window.open();
                    w.document.write(buildPrintContent());
                    w.print();
                    w.close();
                }

                var printButton = document.getElementById("printRecoveryCodes");
                printButton && printButton.addEventListener("click", printRecoveryCodes);
            </script>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
