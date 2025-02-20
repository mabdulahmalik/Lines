<script setup lang="ts">
import { ref, computed } from 'vue';
import Modal from '@/components/modal/Modal.vue';
import { FwbInput, FwbButton, FwbTextarea, FwbToggle, FwbTooltip  } from 'flowbite-vue';
import { IconArrowNarrowDown, IconArrowNarrowUp } from '@/components/icons/index';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import ViewRoutineModal from './modal/ViewRoutineModal.vue';
import CreateRoutineModal from './modal/CreateRoutineModal.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { Routine } from '@/api/__generated__/graphql';

const routinesStore = useRoutinesStore();

const isCreateRoutine = ref<boolean>(true);

// Sorted routines by name
const sortDirection = ref<'asc' | 'desc'>('asc');

const routines = computed(() => {
  const sortedRoutines = [...routinesStore.routines].sort((a: any, b: any) => {
    const nameA = a.name.toUpperCase();
    const nameB = b.name.toUpperCase();
    if (nameA < nameB) return sortDirection.value === 'asc' ? -1 : 1;
    if (nameA > nameB) return sortDirection.value === 'asc' ? 1 : -1;
    return 0;
  });
  return sortedRoutines;
});

const toggleSortDirection = () => {
  sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
};

// Add routine modal
const addRoutineModalRef = ref<InstanceType<typeof Modal>>();

// Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaAddRoutine = yup.object({
  routineName: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitAddRoutine } = useForm({
  validationSchema: schemaAddRoutine,
});

const {
  value: routineName,
  errorMessage: routineNameError,
  resetField: resetRoutineName,
} = useField<string>('routineName');

// Create Routine modal drawer
const createRoutineModalComRef = ref<InstanceType<typeof CreateRoutineModal>>();
const routineDescription = ref('');
const routineNameAsProp = ref('');
const routineDescriptionAsProp = ref('');
function handleAddRoutine() {
  handleSubmitAddRoutine(async () => {
    isCreateRoutine.value = true;
    routineNameAsProp.value = routineName.value;
    routineDescriptionAsProp.value = routineDescription.value;
    const newRoutine = {
      name: routineName.value,
      description: routineDescription.value,
    };
    console.log(newRoutine);
    resetRoutineName();
    routineDescription.value = '';
    createRoutineModalComRef.value?.setModalOpen(true);
    addRoutineModalRef.value?.setModalOpen(false);
  })();
}

const closeRoutineModal = () => {
  addRoutineModalRef.value?.setModalOpen(false);
};

// View Routine modal drawer
const viewRoutineModalComRef = ref<InstanceType<typeof ViewRoutineModal>>();
const handleClickRoutine = (routine: Routine): void => {
  isCreateRoutine.value = false;
  routinesStore.selectedRoutine = routine;
  viewRoutineModalComRef.value?.setModalOpen(true);
};

// Toggle routine status
function onToggleChange(routine: any) {
  if (!routine.isActive) {
    routinesStore.activateRoutine({ id: routine.id });
    return;
  }
  routinesStore.deactivateRoutine({ id: routine.id });
}
</script>

<template>
  <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
    <div class="max-w-3xl mx-auto px-4">
      <!-- title description -->
      <div class="mt-4 lg:mt-10">
        <div class="font-semibold text-2xl leading-9">Routines</div>
        <div class="text-sm font-medium leading-[19.6px]">
          A Routine is a scheduled task that should be regularly performed within a facility to
          maintain operational efficiency, safety, or compliance. It helps ensure that important
          tasks are consistently carried out.
        </div>
        <div class="flex justify-end">
          <button
            @click="addRoutineModalRef?.setModalOpen(true)"
            class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[46px] mb-3.5"
          >
            Add Routine
          </button>
        </div>
      </div>

      <!-- table start -->
      <div class="routines-table mb-4 lg:mb-10">
        <base-table>
          <t-head class="uppercase">
            <t-r>
              <t-h>
                <span
                  @click="toggleSortDirection"
                  class="text-slate-800 inline-flex items-center gap-1 cursor-pointer"
                >
                  <span>Name</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="sortDirection === 'asc'">
                      <IconArrowNarrowUp />
                    </template>
                    <template v-else>
                      <IconArrowNarrowDown />
                    </template>
                  </span>
                </span>
              </t-h>
              <t-h></t-h>
            </t-r>
          </t-head>
          <t-body v-if="routines.length > 0">
            <t-r v-for="(routine, index) in routines" :key="index" class="hover:bg-slate-50">
              <t-d class="font-semibold text-brand-600">
                <a @click="handleClickRoutine(routine)" href="#">
                  {{ routine.name }}
                </a>
              </t-d>
              <t-d>
                <div class="flex justify-end items-center">
                  <fwb-tooltip>
                    <template #trigger>
                      <fwb-toggle
                        :model-value="routine.isActive || false"
                        @change="onToggleChange(routine)"
                        color="purple"
                      />
                    </template>
                    <template #content>
                      <span class="text-sm font-medium">
                        <template v-if="routine.isActive"> De-activate</template>
                        <template v-else> Activate</template>
                      </span>
                    </template>
                  </fwb-tooltip>
                </div>
              </t-d>
            </t-r>
          </t-body>
          <t-body v-else>
            <t-r>
              <t-d colspan="2" class="text-center">No Routines to Display</t-d>
            </t-r>
          </t-body>
        </base-table>
      </div>
      <!-- table end -->
    </div>

    <!-- Add Routine modal -->
    <teleport to="body">
      <Modal ref="addRoutineModalRef" title="Add Routine" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-4 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900">Name</label>
              <fwb-input
                v-model.trim="routineName"
                placeholder="specify the Routine name here"
                :class="routineNameError ? inputErrorClasses : ''"
              />
              <span v-if="routineNameError" class="text-sm text-radical-red-600">{{
                routineNameError
              }}</span>
            </div>

            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Description</label
              >
              <fwb-textarea
                v-model="routineDescription"
                :rows="4"
                label=""
                placeholder="specify the Routine description here"
              />
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeRoutineModal" color="light" pill> Cancel</fwb-button>
            <fwb-button @click="handleAddRoutine" class="bg-primary-600 hover:bg-brand-600" pill>
              Save
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <!-- Create Routine modal drawer componet -->
    <CreateRoutineModal
      ref="createRoutineModalComRef"
      :name="routineNameAsProp"
      :description="routineDescriptionAsProp"
    />
    <!-- View/Edit Routine modal drawer componet -->
    <ViewRoutineModal ref="viewRoutineModalComRef" :routine="routinesStore.selectedRoutine" />
  </div>
</template>

<style scoped>
.routines-table {
  box-shadow: 0px 1px 2px 0px rgba(0, 0, 0, 0.08) !important;
}
</style>
