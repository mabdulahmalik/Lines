<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('saml.post-form.title')}">
            <script>
                window.onload = function() {document.forms[0].submit()};
            </script>
            <p>
                ${msg("saml.post-form.message")}
            </p>
            <form method="post" action="${samlPost.url}">
                <#if samlPost.SAMLRequest??>
                    <@components.formField
                        type="hidden"
                        id="SAMLRequest"
                        name="SAMLRequest"
                        value="${samlPost.SAMLRequest}"
                    />
                </#if>
                <#if samlPost.SAMLResponse??>
                    <@components.formField
                        type="hidden"
                        id="SAMLResponse"
                        name="SAMLResponse"
                        value="${samlPost.SAMLResponse}"
                    />
                </#if>
                <#if samlPost.relayState??>
                    <@components.formField
                        type="hidden"
                        id="RelayState"
                        name="RelayState"
                        value="${samlPost.relayState}"
                    />
                </#if>

                <noscript>
                    <p>
                        ${msg("saml.post-form.js-disabled")}
                    </p>
                    <div class="${properties.btnWrapperClass!}">
                        <@components.button type="submit" variant="primary">
                            ${msg("doContinue")}
                        </@components.button>
                    </div>
                </noscript>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
