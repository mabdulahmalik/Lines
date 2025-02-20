<script lang="ts" setup>
import { ref, computed } from "vue";
import { FwbInput } from "flowbite-vue";
import { IconSearchOutline } from "@/components/icons/index";
import CustomCheckbox from "@/components/form/CustomCheckbox.vue";

const props = defineProps<{
  lastLogin?: Array<{ name: string }>;
  modelValue: Array<{ name: string }>;
}>();
const emit = defineEmits(["update:modelValue"]);

const selectedLastLogin = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit("update:modelValue", val);
  },
});

const isSelected = (lastLoginName: string): boolean => {
  return selectedLastLogin.value.some((item) => item.name === lastLoginName);
};

const updateMultiSelectValue = (lastLoginName: string, isSelected: boolean) => {
  if (isSelected) {
    selectedLastLogin.value.push({ name: lastLoginName });
  } else {
    selectedLastLogin.value = selectedLastLogin.value.filter((item) => item.name !== lastLoginName);
  }
};

// search query
const searchQuery = ref("");
const filteredLastLoginList = computed(() => {
  return (
    props.lastLogin?.filter((last) =>
      last.name.toLowerCase().includes(searchQuery.value.toLowerCase())
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
        v-for="(lastLogin, indx) in filteredLastLoginList"
        :key="indx"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(lastLogin.name) ? lastLogin.name : '']"
          @update:modelValue="
            (newValue) => updateMultiSelectValue(lastLogin.name, newValue.includes(lastLogin.name))
          "
          :label="capitalize(lastLogin.name)"
          :value="lastLogin.name"
          class="gap-0.5"
        />
      </div>
    </div>
  </div>
</template>
