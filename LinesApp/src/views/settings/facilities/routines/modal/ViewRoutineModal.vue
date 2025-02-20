<script setup lang="ts">
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import Modal from '@/components/modal/Modal.vue';
import { useModalStore } from '@/stores/modal';
import { ref, onUnmounted, onMounted, watch } from 'vue';
import { FwbButton, FwbInput } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import RoutinesModalBody from './RoutinesModalBody.vue';
import { Routine } from '@/api/__generated__/graphql';
import {
  IconExpand01,
  IconDotHorizontal,
  IconTrash01,
  IconArrowLeft,
  IconShrink,
  IconX,
} from '@/components/icons/index';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import DrawerHeaderEditName from '@/views/common/DrawerHeaderEditName.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const props = defineProps<{
  routine: Routine;
}>();

const modalStore = useModalStore();
const routinesStore = useRoutinesStore();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

// Routine Name
const routineName = ref('');
onMounted(() => {
  routineName.value = props.routine.name!;
});
watch(
  () => props.routine.name,
  (newValue) => {
    routineName.value = newValue!;
  }
);
function changeName(val: string) {
  routineName.value = val;
}

// submit forms data on save
const RoutineModalBodyRef = ref<InstanceType<typeof RoutinesModalBody>>();
const handleFooterSave = () => {
  RoutineModalBodyRef.value?.submittedData();
};

function handleClosedRoutineModalDrawer() {
  routinesStore.clearSelectedRoutine();
}
function closeRoutineModalDrawer() {
  viewRoutineModalDrawerRef.value?.setModalOpen(false);
}

// Drawer states
const viewRoutineModalDrawerRef = ref<InstanceType<typeof ModalDrawer>>();
const setModalOpen = (value: boolean): void => {
  viewRoutineModalDrawerRef.value?.setModalOpen(value);
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
const isSaveEnabled = ref(false);
function handleComplete(val: boolean) {
  isSaveEnabled.value = val;
}

// Delete Routine Modal

const deleteRoutineModalRef = ref<InstanceType<typeof Modal>>();
const closeDeleteRoutineModal = () => {
  deleteRoutineModalRef.value?.setModalOpen(false);
};

// Validation

const schemaDeleteRoutine = yup.object({
  routineNameForDelete: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitDeleteRoutine } = useForm({
  validationSchema: schemaDeleteRoutine,
});
const {
  value: routineNameForDelete,
  errorMessage: routineNameError,
  resetField: resetRoutineName,
} = useField<string>('routineNameForDelete');

function handleDeleteRoutine() {
  handleSubmitDeleteRoutine(async () => {
    routinesStore.deleteRoutine({ id: props.routine.id! });
    closeDeleteRoutineModal();
    resetRoutineName();
    // emit('close');
    closeRoutineModalDrawer();
  })();
}

onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});
defineExpose({
  setModalOpen,
});
</script>
<template>
  <!-- routine modal drawer -->
  <ModalDrawer
    ref="viewRoutineModalDrawerRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    @close="handleClosedRoutineModalDrawer"
  >
    <template #header>
      <!-- Mobile status badge -->
      <div
        class="lg:hidden w-100 p-2 font-semibold flex justify-center items-center text-center capitalize"
        :class="routine.isActive ? 'bg-[#FCE8F3] text-[#99154B]' : 'text-blue-700 bg-blue-100'"
      >
      {{ routine.isActive ? 'Active' : 'Inactive' }}
      </div>

      <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
        <div class="flex items-center">
          <!-- Mobile close button -->
          <fwb-button
            @click="closeRoutineModalDrawer"
            color="light"
            pill
            square
            size="lg"
            class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
          >
            <IconArrowLeft width="24" height="24" class="text-[#334155]" />
            <span class="sr-only">Close modal</span>
          </fwb-button>

          <!-- Name and edit name from -->
          <DrawerHeaderEditName :name="routineName" @name-updated="changeName" />
        </div>

        <div class="lg:hidden">
          <!-- Mobile More dropdown  -->
          <Dropdown align-to-end class="rounded-lg">
            <template #trigger>
              <fwb-button color="light" pill square>
                <IconDotHorizontal />
              </fwb-button>
            </template>
            <DropdownMenu>
              <DropdownItem
                @click="deleteRoutineModalRef?.setModalOpen(true)"
                class="!text-radical-red-700"
              >
                <IconTrash01 class="mr-2" />
                Delete Routine
              </DropdownItem>
            </DropdownMenu>
          </Dropdown>
        </div>

        <!-- Icon Buttons Desktop only -->
        <div class="hidden lg:flex items-center gap-2 lg:gap-4">
          <!-- status -->
          <span
            class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 capitalize"
            :class="props.routine.isActive ? 'text-blue-700 bg-blue-100' : 'bg-[#FCE8F3] text-[#99154B]' "
            >{{ props.routine.isActive ? 'Active' : 'Inactive' }}</span
          >

          <!-- dropdown -->
          <Dropdown align-to-end class="rounded-lg">
            <template #trigger>
              <fwb-button color="light" pill square>
                <IconDotHorizontal />
              </fwb-button>
            </template>
            <DropdownMenu>
              <DropdownItem
                @click="deleteRoutineModalRef?.setModalOpen(true)"
                class="!text-radical-red-700"
              >
                <IconTrash01 class="mr-2" />
                Delete Routine
              </DropdownItem>
            </DropdownMenu>
          </Dropdown>

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
      <RoutinesModalBody
        ref="RoutineModalBodyRef"
        :is-create="false"
        :name="routineName"
        :routine="props.routine"
        @close="setModalOpen(false)"
        @complete="handleComplete"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeRoutineModalDrawer" color="light" pill> Cancel</fwb-button>
        <fwb-button
          @click="handleFooterSave"
          class="bg-primary-600 hover:bg-brand-600"
          pill
          :disabled="!isSaveEnabled"
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>

  <!-- Delete Routine Confirmation Modal -->
  <teleport to="body">
    <Modal ref="deleteRoutineModalRef" title="Delete Routine" size="lg" :z_index="55">
      <template #body>
        <div class="p-4 lg:p-6">
          <div class="flex flex-col gap-4">
            <div class="text-sm font-medium">
              Deleting this routine will permanently remove it from your account.
            </div>
            <div class="text-sm font-bold">I understand that</div>
            <div class="text-radical-red-500 flex flex-col gap-2 text-xs font-medium">
              <div class="flex sm:items-center gap-2">
                <IconX class="flex-shrink-0" /> This routine will no longer be accessible
              </div>
              <div class="flex sm:items-center gap-2">
                <IconX class="flex-shrink-0" /> Any existing Assignments to Facilities will be removed
                routine
              </div>
              <div class="flex sm:items-center gap-2">
                <IconX class="flex-shrink-0 align-text-top" /> Existing scheduled Jobs will not be affected, but they will not continue to be auto-scheduled thereafter.
              </div>
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium"
                >Confirm by typing
                <span class="text-purple-800 px-0.5">{{ props.routine.name }}</span> below.</label
              >
              <fwb-input
                v-model.trim="routineNameForDelete"
                :class="routineNameError ? inputErrorClasses : ''"
                placeholder="Routine Name in here"
              />
              <span v-if="routineNameError" class="text-sm text-radical-red-600">{{
                routineNameError
              }}</span>
            </div>
          </div>
        </div>
      </template>
      <template #footer>
        <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
          <fwb-button @click="closeDeleteRoutineModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleDeleteRoutine"
            class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
            pill
            :disabled="routineNameForDelete !== props.routine.name"
          >
            Delete Routine
          </fwb-button>
        </div>
      </template>
    </Modal>
  </teleport>
</template>
