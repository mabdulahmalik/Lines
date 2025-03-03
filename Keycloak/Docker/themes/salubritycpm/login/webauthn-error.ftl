<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=true; section>
    <#if section = "form">
        <@components.cardMain header="${kcSanitize(msg('webauthn-error-title'))?no_esc}">
            <script type="text/javascript">
                refreshPage = () => {
                    document.getElementById('isSetRetry').value = 'retry';
                    document.getElementById('executionValue').value = '${execution}';
                    document.getElementById('error-credential-form').submit();
                }
            </script>

            <form id="error-credential-form" action="${url.loginAction}" method="post">
                <@components.formField
                    type="hidden"
                    id="executionValue"
                    name="authenticationExecution"
                />
                <@components.formField
                    type="hidden"
                    id="isSetRetry"
                    name="isSetRetry"
                />
            </form>

            <div class="${properties.btnWrapperClass!}">
                <@components.button type="button" variant="primary" onclick="refreshPage()" name="try-again">
                    ${kcSanitize(msg("doTryAgain"))?no_esc}
                </@components.button>
            </div>

            <#if isAppInitiatedAction??>
                <form action="${url.loginAction}" method="post">
                   <div class="${properties.btnWrapperClass!}">
                       <@components.button type="submit" variant="default" id="cancelWebAuthnAIA" name="cancel-aia">
                           ${msg("doCancel")}
                       </@components.button>
                   </div>
                </form>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>