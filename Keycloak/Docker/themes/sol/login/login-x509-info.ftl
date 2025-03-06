<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('doLogIn')}">
            <form action="${url.loginAction}" method="post">

                <label for="certificate_subjectDN">${msg("clientCertificate")}</label>
                <#if x509.formData.subjectDN??>
                    <label id="certificate_subjectDN">${(x509.formData.subjectDN!"")}</label>
                <#else>
                    <label id="certificate_subjectDN">${msg("noCertificate")}</label>
                </#if>


                <#if x509.formData.isUserEnabled??>
                    <label for="username">${msg("doX509Login")}</label>
                    <label id="username">${(x509.formData.username!'')}</label>
                </#if>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submmit" variant="primary" name="login">
                        ${msg("doContinue")}
                    </@components.button>
                    <#if x509.formData.isUserEnabled??>
                        <@components.button type="submmit" variant="secondary" name="cancel">
                            ${msg("doIgnore")}
                        </@components.button>
                    </#if>
                </div>
            </form>
        </@components.cardMain>
    </#if>

</@layout.registrationLayout>
