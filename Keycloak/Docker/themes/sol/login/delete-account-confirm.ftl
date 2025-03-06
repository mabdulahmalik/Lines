<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('deleteAccountConfirm')}">
            <form action="${url.loginAction}" method="post">
                <@components.alertWarning>
                    ${msg("irreversibleAction")}
                </@components.alertWarning>

                <p>${msg("deletingImplies")}</p>
                <ul>
                    <li>${msg("loggingOutImmediately")}</li>
                    <li>${msg("errasingData")}</li>
                </ul>
                <p>
                    ${msg("finalDeletionConfirmation")}
                </p>

                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" variant="danger">
                        ${msg("doConfirmDelete")}
                    </@components.button>
                    <#if triggered_from_aia>
                        <@components.button type="submit" variant="secondary" name="cancel-aia" value="true">
                            ${msg("doCancel")}
                        </@components.button>
                    </#if>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>