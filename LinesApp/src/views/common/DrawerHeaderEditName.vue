<script setup lang="ts">
import { ref } from 'vue';
import { FwbInput, FwbButton } from 'flowbite-vue';
import { IconEdit03 } from '@/components/icons/index';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const props = defineProps<{ name: string }>();
const emit = defineEmits(['name-updated']);

const name = ref(props.name);
const isEditName = ref(false);

// Valitiation for edit name
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaEditRoutineName = yup.object({
  nameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit } = useForm({
  validationSchema: schemaEditRoutineName,
});

const { value: nameEdit, errorMessage: nameEditError } = useField<string>('nameEdit');


const handleClickNameEditIcon = () => {
  nameEdit.value = name.value;
  isEditName.value = true;
};
const handleSave = () => {
  handleSubmit(async () => {
    name.value = nameEdit.value;
    isEditName.value = false;
    emit('name-updated', name.value);
  })();
};
</script>

<template>
  <div v-if="isEditName" class="flex items-center gap-3">
    <fwb-input
      size="sm"
      v-model.trim="nameEdit"
      placeholder="Write text here"
      class="sm:w-[295px]"
      :class="nameEditError ? inputErrorClasses : ''"
    />
    <div class="flex items-center gap-3">
      <fwb-button @click="isEditName = false" size="sm" color="light" pill> Cancel</fwb-button>
      <fwb-button @click="handleSave" size="sm" class="bg-primary-600 hover:bg-brand-600" pill
        >Confirm</fwb-button
      >
    </div>
  </div>
  <div v-else class="flex items-center gap-3">
    <h2 class="text-lg lg:text-2xl font-semibold">
      {{ name }}
    </h2>
    <fwb-button
      @click.stop="handleClickNameEditIcon"
      color="light"
      pill
      square
      class="border-transparent"
    >
      <IconEdit03 />
    </fwb-button>
  </div>
</template>
