<script lang="ts" setup>
import { ref, computed } from "vue";
import { FwbInput } from "flowbite-vue";
import { IconSearchOutline } from "@/components/icons/index";
import CustomCheckbox from "@/components/form/CustomCheckbox.vue";

const props = defineProps<{
  role?: Array<{ name: string }>;
  modelValue: Array<{ name: string }>;
}>();
const emit = defineEmits(["update:modelValue"]);

const selectedRole = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit("update:modelValue", val);
  },
});

const isSelected = (roleName: string): boolean => {
  return selectedRole.value.some((item) => item.name === roleName);
};

const updateMultiSelectValue = (roleName: string, isSelected: boolean) => {
  if (isSelected) {
    selectedRole.value.push({ name: roleName });
  } else {
    selectedRole.value = selectedRole.value.filter((item) => item.name !== roleName);
  }
};

// search query
const searchQuery = ref("");
const filteredRoleList = computed(() => {
  return (
    props.role?.filter((r) => r.name.toLowerCase().includes(searchQuery.value.toLowerCase())) || []
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
        v-for="(role, indx) in filteredRoleList"
        :key="indx"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(role.name) ? role.name : '']"
          @update:modelValue="
            (newValue) => updateMultiSelectValue(role.name, newValue.includes(role.name))
          "
          :label="capitalize(role.name)"
          :value="role.name"
          class="gap-0.5"
        />
      </div>
    </div>
  </div>
</template>
