<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('logoutConfirmTitle')}">
            <p>
                ${msg("logoutConfirmHeader")}
            </p>

            <form action="${url.logoutConfirmAction}" method="post">
                <@components.formField
                    type="hidden"
                    id="session_code"
                    name="session_code"
                    value="${logoutConfirm.code}"
                />

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="primary" name="confirmLogout">
                        ${msg("doLogout")}
                    </@components.button>
                </div>
            </form>

            <#if logoutConfirm.skipLink>
            <#else>
                <#if (client.baseUrl)?has_content>
                    <hr>
                    <div class="${properties.textCenterClass!}">
                        <@components.link href="${client.baseUrl}">
                            ${kcSanitize(msg("backToApplication"))?no_esc}
                        </@components.link>
                    </div>
                </#if>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
