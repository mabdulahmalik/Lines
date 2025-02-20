<script setup lang="ts">
import { onMounted, onUnmounted, computed, ref, nextTick, watch } from 'vue';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import draggable from 'vuedraggable';
import { IconArrowNarrowDown, IconDotsReorder, IconChevronDown } from '@/components/icons/index';
import { FwbButton, FwbButtonGroup } from 'flowbite-vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import AddFacilityModal from './partials/modal/AddFacilityModal.vue';
import ViewFacilityModal from './partials/modal/ViewFacilityModal.vue';
import RoomsCount from './partials/roomsCounts/RoomsCount.vue';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { Facility } from '@/api/__generated__/graphql';
import { useStatesStore } from '@/stores/data/location/states';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';

const breadcrumbStore = useBreadcrumbStore();
const facilitiesStore = useFacilitiesStore();
const statesStore = useStatesStore();
const facilityTypesStore = useFacilityTypesStore();

onMounted(() => {
  breadcrumbStore.breadcrumbs = [{ title: 'Facilities' }];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const facilities = ref([...facilitiesStore.facilities]);
const activeFacilities = ref<Facility[]>([]);
const archivedFacilities = ref<Facility[]>([]);

const filteredFacilities = computed(() => {
  return facilities.value.filter((facility) => {
    const matchesStatus =
      (!facility.archived && facilitiesStatus.value === 'Active') ||
      (facility.archived && facilitiesStatus.value === 'Archived');
    return matchesStatus;
  });
});

watch(
  () => facilitiesStore.facilities,
  (newFacilities) => {
    facilities.value = [...newFacilities];
    activeFacilities.value = newFacilities.filter((facility) => !facility.archived);
    archivedFacilities.value = newFacilities.filter((facility) => facility.archived);
    if (archivedFacilities.value.length < 1) {
      facilitiesStatus.value = 'Active';
    }
  },
  { deep: true }
);

onMounted(() => {
  activeFacilities.value = facilitiesStore.facilities.filter((facility) => !facility.archived);
  archivedFacilities.value = facilitiesStore.facilities.filter((facility) => facility.archived);
});

// Get Facility Type By Id
const getFacilityTypeById = (facilityId: string) => {
  return facilityTypesStore.facilityTypes.find((type) => type.id === facilityId)?.name;
};

// Filters Active & Archived
const facilitiesStatus = ref('Active');
function togglesFacilities(val: string) {
  facilitiesStatus.value = val;
}

// Drag and drop Facilities
const onDragFacilities = (evt: any) => {
  const { oldIndex, newIndex } = evt;
  const draggedItemId = evt.item.dataset.id;
  const payload = {
    from: oldIndex + 1,
    id: draggedItemId,
    to: newIndex + 1,
  };
  facilitiesStore.sortFacilities(payload);
  console.log('Item position changed:', payload);
};

//  Add Facility Modal Drawer
const addFacilityModalComponentRef = ref<InstanceType<typeof AddFacilityModal>>();
const isAddFacilityModalVisible = ref(false);
function handleAddFacilityModal() {
  isAddFacilityModalVisible.value = true;
  nextTick(() => {
    addFacilityModalComponentRef.value?.setModalOpen(true);
  });
}

// View Facility Modal Drawer
const viewFacilityModalComponentRef = ref<InstanceType<typeof ViewFacilityModal>>();
const isViewFacilityModalVisible = ref(false);

function handleViewFacilityModal(facility: Facility) {
  isViewFacilityModalVisible.value = true;
  facilitiesStore.selectedFacility = facility;
  nextTick(() => {
    viewFacilityModalComponentRef.value?.setModalOpen(true);
  });
}

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
  <div
    class="p-4 lg:p-10 overflow-auto max-h-[calc(100vh-193px)] lg:max-h-[calc(100vh-80px)]"
    style="max-width: 100vw"
  >
    <!-- Table action tab -->
    <div
      class="flex sm:gap-4 gap-6 sm:justify-between items-center sm:bg-transparent bg-white sm:py-[14px] py-4 sm:px-0 px-6 fixed w-full sm:relative bottom-0 left-0 right-0 sm:border-none border-t border-b z-10"
    >
      <div v-if="archivedFacilities.length">
        <!-- Filters for Desktop -->
        <fwb-button-group class="sm:block hidden shadow-none text-nowrap">
          <fwb-button
            @click="togglesFacilities('Active')"
            color="light"
            class="focus:ring-0 hover:bg-transparent focus:bg-slate-100"
            >Active <span>({{ activeFacilities.length }})</span></fwb-button
          >
          <fwb-button
            @click="togglesFacilities('Archived')"
            color="light"
            class="focus:ring-0 hover:bg-transparent focus:bg-slate-100 border-l-0"
            >Archived <span>({{ archivedFacilities.length }})</span></fwb-button
          >
        </fwb-button-group>

        <!-- Filters for Mobile -->
        <div class="sm:hidden relative min-w-[138px] w-full flex items-center">
          <select
            class="w-ful bg-transparent border-0 text-sm font-semibold text-slate-800 focus:outline-none focus:ring-0"
            style="background: transparent"
            v-model="facilitiesStatus"
          >
            <option value="Active">
              Active <span>({{ activeFacilities.length }})</span>
            </option>
            <option value="Archived">
              Archived <span>({{ archivedFacilities.length }})</span>
            </option>
          </select>
          <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2">
            <IconChevronDown />
          </div>
        </div>
      </div>
      <div class="sm:w-auto w-full ml-auto">
        <button
          @click="handleAddFacilityModal"
          class="bg-brand-700 sm:py-2 py-2.5 sm:px-3 px-5 rounded-full text-white text-sm font-semibold sm:w-auto w-full"
        >
          Add Facility
        </button>
      </div>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto">
      <base-table>
        <t-head class="uppercase">
          <t-r>
            <t-h class="w-6"></t-h>
            <t-h class="w-16">
              <div class="text-slate-800 flex items-center gap-2">
                <span>Order</span>
                <IconArrowNarrowDown />
              </div>
            </t-h>
            <t-h class="min-w-48">Name </t-h>
            <t-h class="min-w-48 text-nowrap">TYPE</t-h>
            <t-h class="min-w-28">Rooms </t-h>
            <t-h class="min-w-52 text-nowrap">Location </t-h>
            <t-h class="min-w-52 text-nowrap">Time zone</t-h>
          </t-r>
        </t-head>
        <draggable
          v-if="filteredFacilities.length > 0"
          v-model="filteredFacilities"
          item-key="id"
          tag="tbody"
          v-bind="dragOptions"
          handle=".draggable-handle-icon"
          @end="onDragFacilities"
        >
          <template #item="{ element: facility, index }">
            <t-r :data-id="facility.id" class="hover:bg-slate-50">
              <t-d class="w-6">
                <IconDotsReorder class="cursor-grab active:cursor-grabbing draggable-handle-icon" :class="facility.archived ? 'pointer-events-none':''" />
              </t-d>
              <t-d>{{ index + 1 }}</t-d>
              <t-d class="text-brand-600 font-semibold">
                <a @click="handleViewFacilityModal(facility)" href="#">
                  {{ facility.name }}
                </a>
              </t-d>
              <t-d>
                <span
                  class="text-xs bg-[#DEF7EC] text-[#057A55] px-[10px] py-[2px] leading-[18px] font-medium rounded-full text-nowrap"
                >
                  {{ getFacilityTypeById(facility.typeId) }}
                </span>
              </t-d>
              <t-d>
                <RoomsCount :facilityId="facility.id" />
              </t-d>
              <t-d>
                {{ facility.address.city }},
                {{ statesStore.getStateFullName(facility.address.state) }}
              </t-d>
              <t-d>
                {{ facility.timeZone }}
              </t-d>
            </t-r>
          </template>
        </draggable>
        <t-body v-else>
          <t-r>
            <t-d colspan="7" class="text-center">No Facilities to Display</t-d>
          </t-r>
        </t-body>
      </base-table>
    </div>
    <!-- Add Facility Modal Drawer -->
    <AddFacilityModal v-if="isAddFacilityModalVisible" ref="addFacilityModalComponentRef" />
    <!-- View Facility Modal Drawer -->
    <ViewFacilityModal
      v-if="isViewFacilityModalVisible"
      ref="viewFacilityModalComponentRef"
      :facility="facilitiesStore.selectedFacility"
    />
  </div>
</template>
