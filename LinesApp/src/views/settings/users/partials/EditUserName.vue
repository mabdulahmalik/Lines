<script setup lang="ts">
import { ref } from 'vue';
import { FwbInput, FwbButton } from 'flowbite-vue';
import { IconEdit03 } from '@/components/icons/index';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const props = defineProps<{ fName: string; lName: string }>();
const emit = defineEmits(['name-updated']);

const fName = ref(props.fName);
const lName = ref(props.lName);

const isEditName = ref(false);

// Valitiation for edit name
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaEditRoutineName = yup.object({
  fNameEdit: yup.string().required('This is a required field'),
  lNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit } = useForm({
  validationSchema: schemaEditRoutineName,
});

const { value: fNameEdit, errorMessage: fNameEditError } = useField<string>('fNameEdit');
const { value: lNameEdit, errorMessage: lNameEditError } = useField<string>('lNameEdit');

const handleClickNameEditIcon = () => {
  fNameEdit.value = fName.value;
  lNameEdit.value = lName.value;
  isEditName.value = true;
};
const handleSave = () => {
  handleSubmit(async () => {
    fName.value = fNameEdit.value;
    lName.value = lNameEdit.value;
    isEditName.value = false;
    emit('name-updated', { fName: fName.value, lName: lName.value });
  })();
};
</script>

<template>
  <div v-if="isEditName" class="flex items-center gap-3">
    <fwb-input
      size="sm"
      v-model.trim="fNameEdit"
      placeholder="First Name"
      class="sm:w-[200px]"
      :class="fNameEditError ? inputErrorClasses : ''"
    />
    <fwb-input
      size="sm"
      v-model.trim="lNameEdit"
      placeholder="Last Name"
      class="sm:w-[200px]"
      :class="lNameEditError ? inputErrorClasses : ''"
    />
    <div class="flex items-center gap-3">
      <fwb-button @click="isEditName = false" size="sm" color="light" pill> Cancel</fwb-button>
      <fwb-button @click="handleSave" size="sm" class="bg-primary-600 hover:bg-brand-600" pill
        >Confirm</fwb-button
      >
    </div>
  </div>
  <div v-else class="flex items-center gap-3">
    <h2 class="text-lg lg:text-2xl font-semibold">{{ fName }} {{ lName }}</h2>
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
