<script setup lang="ts">
import Tabs from '@/components/tabs/Tabs.vue';
import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
import Modal from '@/components/modal/Modal.vue';
import { FwbInput, FwbButton, FwbToggle, FwbTooltip } from 'flowbite-vue';
import { IconArrowNarrowDown, IconDotsReorder } from '@/components/icons/index';
import { BaseTable, THead, TR, TD, TH, TBody } from '@/components/table/index';
import draggable from 'vuedraggable';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { FacilityType } from '@/api/__generated__/graphql';
import Routines from './routines/Routines.vue';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useTabStore } from '@/stores/tab';
import ViewFacilityModal from './modal/ViewFacilityModal.vue';

// Stores
const breadcrumbStore = useBreadcrumbStore();
const facilityTypesStore = useFacilityTypesStore();
const routinesStore = useRoutinesStore();
const tabStore = useTabStore();

// Breadcrumbs settings
onMounted(() => {
  tabStore.settingsFacilitiesActiveTab = 0;
});

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Settings', to: '/settings/encounters' },
    { title: 'Facilities' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

// Tabs
const fTCount = computed(() => facilityTypesStore.facilityTypes?.length);
const routinesCount = computed(() => routinesStore.routines?.length);
const tabs = computed(() => [
  {
    label: 'Facility Types',
    badge: fTCount.value,
  },
  {
    label: 'Routines',
    badge: routinesCount.value,
  },
]);

// Add facility type
const modalFacilityRef = ref<InstanceType<typeof Modal>>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaFacilityType = yup.object({
  facilityTypeName: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSaveFacility } = useForm({
  validationSchema: schemaFacilityType,
});
const {
  value: facilityTypeName,
  errorMessage: facilityTypeNameError,
  resetField: resetFacilityTypeName,
} = useField<string>('facilityTypeName');

function handleCancelFacility() {
  modalFacilityRef.value?.setModalOpen(false);
}
function handleAddFacility() {
  handleSaveFacility(async () => {
    const newFacility = {
      name: facilityTypeName.value,
    };
    facilityTypesStore.createFacilityType(newFacility);
    resetFacilityTypeName();
    modalFacilityRef.value?.setModalOpen(false);
  })();
}

// Facility Types
const viewFacilityModalComRef = ref<InstanceType<typeof ViewFacilityModal>>();
const facilityTypes = ref([...facilityTypesStore.facilityTypes]);

watch(
  () => facilityTypesStore.facilityTypes,
  (newValue) => {
    facilityTypes.value = [...newValue];
  },
  { deep: true }
);

const hadleClickFacilityType = (ft: FacilityType) => {
  facilityTypesStore.selectedFacilityType = ft;
  viewFacilityModalComRef.value?.setModalOpen(true);
};

// activate/deactivated
function onToggleChange(facilityType: any) {
  if (!facilityType.isActive) {
    facilityTypesStore.activateFacilityType({ id: facilityType.id });
    return;
  }
  facilityTypesStore.deactivateFacilityType({ id: facilityType.id });
}

// Sort facility types
const onDragEndFacilityTypes = (evt: any) => {
  const { oldIndex, newIndex } = evt;
  const draggedItemId = evt.item.dataset.id;
  const payload = {
    from: oldIndex + 1,
    id: draggedItemId,
    to: newIndex + 1,
  };
  facilityTypesStore.sortFacilityType(payload);
};

// drag & drop
const dragOptions = computed(() => {
  return {
    animation: 200,
    group: 'table-items',
    disabled: false,
    ghostClass: 'shadow',
  };
});
</script>
<template>
  <div class="lg:py-10 py-4">
    <Tabs :tabs="tabs" :active-tab="tabStore.settingsFacilitiesActiveTab" margin="lg:mx-10 mx-4">
      <!-- Facility Types tab -->
      <template #tab-0>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-3xl mx-auto lg:0 px-4">
            <div class="mt-4 lg:mt-10">
              <div class="font-semibold text-2xl leading-9">Facility Types</div>
              <div class="text-sm font-medium leading-[19.6px]">
                Facility Types help to classify the different facilities being serviced by your
                team. They contain Room Properties that can be used to describe and define the
                organizational structure of your facilities.
              </div>
              <div class="flex justify-end">
                <button
                  @click="modalFacilityRef?.setModalOpen(true)"
                  class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[38px] mb-3.5"
                >
                  Add Facility Type
                </button>
              </div>
            </div>
            <div class="mb-4 lg:mb-10">
              <base-table>
                <t-head class="uppercase">
                  <t-r>
                    <t-h class="w-6"></t-h>
                    <t-h class="w-16">
                      <div class="text-slate-800 flex items-center gap-2">
                        <span>Order </span>
                        <IconArrowNarrowDown />
                      </div>
                    </t-h>
                    <t-h>Name</t-h>
                    <t-h>Room Properties</t-h>
                    <t-h></t-h>
                  </t-r>
                </t-head>
                <draggable
                  v-if="facilityTypes.length > 0"
                  v-model="facilityTypes"
                  item-key="id"
                  tag="tbody"
                  v-bind="dragOptions"
                  handle=".draggable-handle-icon"
                  @end="onDragEndFacilityTypes"
                >
                  <template #item="{ element: item, index }">
                    <t-r :data-id="item.id" class="hover:bg-slate-50">
                      <t-d class="w-6">
                        <IconDotsReorder
                          class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                      /></t-d>
                      <t-d> {{ index + 1 }}</t-d>
                      <t-d class="text-brand-600 font-semibold">
                        <a @click="hadleClickFacilityType(item)" href="#">
                          {{ item.name }}
                        </a>
                      </t-d>
                      <t-d class="text-brand-600 font-semibold">{{ item.properties.length }}</t-d>
                      <t-d>
                        <div class="flex justify-end items-center">
                          <fwb-tooltip>
                            <template #trigger>
                              <FwbToggle
                                :model-value="item.isActive"
                                @change="onToggleChange(item)"
                                color="purple"
                              />
                            </template>
                            <template #content>
                              <span class="text-sm font-medium">
                                <template v-if="item.isActive"> De-activate</template>
                                <template v-else> Activate</template>
                              </span>
                            </template>
                          </fwb-tooltip>
                        </div>
                      </t-d>
                    </t-r>
                  </template>
                </draggable>
                <t-body v-else>
                  <t-r>
                    <t-d colspan="4" class="text-center">No Facility Types to Display</t-d>
                  </t-r>
                </t-body>
              </base-table>
            </div>
          </div>
        </div>
      </template>
      <!-- Routines tab -->
      <template #tab-1>
        <Routines />
      </template>
    </Tabs>

    <!-- Add facility type modal -->
    <teleport to="body">
      <Modal ref="modalFacilityRef" title="Add Facility Type" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="facilityTypeName"
                placeholder="input name here ..."
                :class="facilityTypeNameError ? inputErrorClasses : ''"
              />
              <span v-if="facilityTypeNameError" class="text-sm text-radical-red-600">{{
                facilityTypeNameError
              }}</span>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelFacility" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleAddFacility"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Save & Close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>

    <!-- View Facility type modal -->
    <ViewFacilityModal
      ref="viewFacilityModalComRef"
      :facilityType="facilityTypesStore.selectedFacilityType"
    />
  </div>
</template>