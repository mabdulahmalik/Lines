<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>
<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${code.success?then(msg('codeSuccessTitle'), kcSanitize(msg("codeErrorTitle", code.error)))}">
            <#if code.success>
                <p>
                    ${msg("copyCodeInstruction")}
                </p>
                <@components.formField
                    type="text"
                    id="code"
                    name="code"
                    value="${code.code}"
                />
            <#else>
                <p>
                    ${kcSanitize(code.error)}
                </p>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
