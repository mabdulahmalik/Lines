<script setup lang="ts">
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import CreateProcedureModalBody from './CreateProcedureModalBody.vue';
import { useModalStore } from '@/stores/modal';
import { ref, onUnmounted, onMounted, watch } from 'vue';
import { FwbInput, FwbButton } from 'flowbite-vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import {
  IconEdit03,
  IconExpand01,
  IconArrowLeft,
  IconShrink,
} from '@/components/icons/index';
import { ProcedureType } from '@/api/__generated__/graphql';

const props = defineProps<{
  name: string;
  type: string;
}>();
const modalStore = useModalStore();

// Valitiation
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaEditProcedureName = yup.object({
  procedureNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmittedProcedureName } = useForm({
  validationSchema: schemaEditProcedureName,
});

// Procedure Name
const procedureName = ref('');
onMounted(()=>{
  procedureName.value = props.name;
})
watch(
  () => props.name,
  (newValue) => {
    procedureName.value = newValue;
  }
);
const { value: procedureNameEdit, errorMessage: procedureNameEditError } =
  useField<string>('procedureNameEdit');
const isEditProcedureName = ref(false);
const handleClickProcedureNameEditIcon = () =>{
  procedureNameEdit.value = procedureName.value;
  isEditProcedureName.value = true;
}
const cancelEditProcedureName = () => {
  isEditProcedureName.value = false;
};

const saveEditProcedureName = () => {
  handleSubmittedProcedureName(async () => {
    procedureName.value = procedureNameEdit.value;
    isEditProcedureName.value = false;
  })();
};




// submit forms data on save
const CreateProcedureModalBodyRef = ref<InstanceType<typeof CreateProcedureModalBody>>();

const handleSaveForms = () => {
  CreateProcedureModalBodyRef.value?.submittedData();
};
function closeProcedureModalDrawer() {
  setModalOpen(false);
}



// Drawer States
const modalCreateProcedureRef = ref<InstanceType<typeof ModalDrawer>>();
const setModalOpen = (value: boolean): void => {
  modalCreateProcedureRef.value?.setModalOpen(value);
};
const fullWidth = ref(false);
const modalWidth = ref('5xl');
const setModalWidth = (val: string) => {
  modalWidth.value = val;
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isModalDrawerExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isModalDrawerExpended = false;
  }
};
onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});
defineExpose({
  setModalOpen,
});
</script>
<template>
  <!-- create procedures modal drawer -->
  <ModalDrawer
    ref="modalCreateProcedureRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
  >
    <template #header>
      <!-- Removal/insertion badge mobile -->
      <div
        v-if="props.type === ProcedureType.Insertion"
        class="lg:hidden w-100 p-2 font-semibold flex justify-center items-center text-center text-green-700 bg-green-100"
      >
        Insertion
      </div>
      <div
        v-else-if="props.type === ProcedureType.Removal"
        class="lg:hidden w-100 p-2 font-semibold flex justify-center items-center text-center text-red-700 bg-red-100"
      >
        Removal
      </div>
      <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
        <div class="flex items-center">
          <!-- Mobile close button -->
          <fwb-button
            @click="closeProcedureModalDrawer"
            color="light"
            pill
            square
            size="lg"
            class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
          >
            <IconArrowLeft width="24" height="24" class="text-slate-700" />
            <span class="sr-only">Close modal</span>
          </fwb-button>

          <!-- edit procedure name -->
          <div v-if="isEditProcedureName" class="flex items-center gap-3">
            <fwb-input
              v-model.trim="procedureNameEdit"
              size="sm"
              placeholder="Write text here"
              class="sm:w-[295px]"
              :class="procedureNameEditError ? inputErrorClasses : ''"
            />
            <div class="flex items-center gap-3">
              <fwb-button @click="cancelEditProcedureName" size="sm" color="light" pill>
                Cancel</fwb-button
              >
              <fwb-button
                @click="saveEditProcedureName"
                size="sm"
                class="bg-primary-600 hover:bg-brand-600"
                pill
                >Confirm</fwb-button
              >
            </div>
          </div>
          <div v-else class="flex items-center gap-3">
            <h2 class="text-lg lg:text-2xl font-semibold">
              {{ procedureName }}
            </h2>
            <fwb-button
              @click.stop="handleClickProcedureNameEditIcon"
              color="light"
              pill
              square
              class="border-transparent"
            >
              <IconEdit03 />
            </fwb-button>
          </div>
        </div>

        <!-- Icon Buttons Desktop only -->
        <div class="hidden lg:flex items-center gap-2 lg:gap-4">
          <!--Insertion/removal badge -->
          <span
            v-if="props.type === ProcedureType.Insertion"
            class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 bg-green-100 text-green-700"
            >Insertion</span
          >
          <span
            v-else-if="props.type === ProcedureType.Removal"
            class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 bg-red-100 text-red-700"
            >Removal</span
          >
          <!-- Expand -->
          <fwb-button v-if="!fullWidth" @click="setModalWidth('full')" color="light" pill square>
            <IconExpand01 />
          </fwb-button>
          <!-- Shrink -->
          <fwb-button v-if="fullWidth" @click="setModalWidth('5xl')" color="light" pill square>
            <IconShrink />
          </fwb-button>
        </div>
      </div>
    </template>
    <template #body class="bg-red-200">
      <CreateProcedureModalBody ref="CreateProcedureModalBodyRef" :name="procedureName" :type="props.type" @close="setModalOpen(false)"/>
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeProcedureModalDrawer" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleSaveForms" class="bg-primary-600 hover:bg-brand-600" pill
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>
</template>
