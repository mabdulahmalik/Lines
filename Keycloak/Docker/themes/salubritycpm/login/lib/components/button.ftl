<#macro button type="button" variant="" disabled=false props...>
    <button
            <#switch variant>
                <#case "primary">
                    class="my-4 py-2 px-4 w-full rounded-full bg-button-primary-bg text-button-primary-text font-semibold"
                <#break>
                <#case "secondary">
                    class="my-4 py-2 px-4 w-full rounded-full bg-button-secondary-bg text-button-secondary-text font-semibold"
                <#break>
                <#case "info">
                    class="my-4 py-2 px-4 w-full rounded-full bg-button-secondary-bg text-button-secondary-text font-semibold"
                <#break>
                <#case "danger">
                    class="my-4 py-2 px-4 w-full rounded-full bg-button-secondary-bg text-button-secondary-text font-semibold"
                <#break>
                <#default>
                    class="my-4 py-2 px-4 w-full rounded-full bg-button-secondary-bg text-button-secondary-text font-semibold"
            </#switch>

            type="${type}"
            ${disabled?then("disabled", "")}

            <#list props as attrName, attrVal>
                ${attrName}="${attrVal}"
            </#list>
    >
        <#nested>
    </button>
</#macro>