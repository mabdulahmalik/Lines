<script lang="ts" setup>
import { ref, computed } from "vue";
import { FwbInput } from "flowbite-vue";
import { IconSearchOutline } from "@/components/icons/index";
import CustomCheckbox from "@/components/form/CustomCheckbox.vue";

const props = defineProps<{
  identities?: Array<{ name: string }>;
  modelValue: Array<{ name: string }>;
}>();
const emit = defineEmits(["update:modelValue"]);

const selectedIdentities = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit("update:modelValue", val);
  },
});

const isSelected = (identityName: string): boolean => {
  return selectedIdentities.value.some((item) => item.name === identityName);
};

const updateMultiSelectValue = (identityName: string, isSelected: boolean) => {
  if (isSelected) {
    selectedIdentities.value.push({ name: identityName });
  } else {
    selectedIdentities.value = selectedIdentities.value.filter(
      (item) => item.name !== identityName
    );
  }
};

// search query
const searchQuery = ref("");
const filteredIdentityList = computed(() => {
  return (
    props.identities?.filter((identity) =>
      identity.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    ) || []
  );
});

const capitalize = (text: string): string => {
  return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
};
</script>

<template>
  <div class="flex flex-col gap-4 lg:gap-6">
    <fwb-input v-model.trim="searchQuery" placeholder="Search">
      <template #prefix>
        <IconSearchOutline width="20" height="20" class="text-gray-500 dark:text-gray-400" />
      </template>
    </fwb-input>

    <div class="flex flex-col gap-4">
      <div
        v-for="(identity, indx) in filteredIdentityList"
        :key="indx"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(identity.name) ? identity.name : '']"
          @update:modelValue="
            (newValue) => updateMultiSelectValue(identity.name, newValue.includes(identity.name))
          "
          :label="capitalize(identity.name)"
          :value="identity.name"
          class="gap-0.5"
        />
      </div>
    </div>
  </div>
</template>
