<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { FwbBadge } from 'flowbite-vue';
import { IconClockFastForward, IconArrowCircleBrokenDownRight, IconAlertHexagon, IconClockCheck} from '../icons';
import { Encounter, EncounterStage, Job, JobStatus, FacilityRoom, User } from '@/api/__generated__/graphql';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useUsersStore } from '@/stores/data/settings/users';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { useEncounterTimers } from '@/hooks/useEncounterTimers';
import UserAvatar from '@/components/avatar/UserAvatar.vue';

const props = defineProps<{
  job: Job;
  encounter?: Encounter;
  minimal?: boolean;
}>();

const purposesStore = usePurposesStore();
const facilitiesStore = useFacilitiesStore();
const usersStore = useUsersStore();
const linesStore = useLinesStore()

const { relativeTimeJob, elapsedTime, attendingDuration } = useEncounterTimers(props);


const badgeColor = computed(() => {
  let color;
  switch (props.job.status) {
    case JobStatus.Scheduled:
      color = 'green';
      break;
    case JobStatus.Completed:
      color = 'green';
      break;
    case JobStatus.Underway:
      color = 'brand';
      break;
    case JobStatus.Canceled:
      color = 'gray';
      break;
    default:
      color = 'slate';
  }
  return color;
});

function getPurposeNameById(id: string): string {
  const purpose = purposesStore.purposes.find((p) => p.id === id);
  return purpose?.name ?? '';
}
function getFacilityNameById(id: string): string {
  const facility = facilitiesStore.facilities.find((f) => f.id === id);
  return facility?.name ?? '';
}

onMounted(() => {
  const facilityId = props.job.location?.facilityId;
  if (facilityId) {
    getRoomsForFacility(facilityId);
  }
});
const roomsCache = ref<FacilityRoom[]>([]);

function getRoomsForFacility(facilityId: string): void {
  facilitiesStore.getRooms(facilityId)((result) => {
    if (result.data && result.data.facilityRooms) {
      roomsCache.value = result.data.facilityRooms.items?.filter((room) => room !== null) || [];
    }
  });
}
function getRoomNameById(roomId: string): string {
  const room = roomsCache.value.find((r) => r.id === roomId);
  return room?.name ?? '';
}

function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}


// Encounter/Job Timers

// Associated Lines
const associatedLines = computed(() => {
  return linesStore.lines.filter((line) => props.encounter?.lines?.includes(line.id));
});

const externallyPlaced = computed(() => {
  return associatedLines.value.some((line) => line.externallyPlaced);
});
const hasInfection = computed(() => {
  return associatedLines.value.some((line) => line.infectedOn);
});
</script>
<template>
  <div
    class="rounded-lg border bg-white cursor-pointer border-slate-300 hover:border-brand-600 hover:shadow-md duration-300 ease-in-out"
  >
    <div class="p-4 text-slate-500">
      <div class="flex lg:items-center justify-between flex-col lg:flex-row gap-4">
        <div>
          <h3 class="text-base font-semibold text-brand-600">
            {{ getPurposeNameById(props.job.purposeId) }}
          </h3>
          <p class="text-sm font-medium">
            {{ getFacilityNameById(props.job.location?.facilityId) }} &nbsp;-&nbsp;
            {{ getRoomNameById(props.job.location?.roomId) }}
          </p>
        </div>
        <div class="flex items-center gap-3">
          <!-- Attanding Timer -->
          <fwb-badge
            v-if="encounter?.stage === EncounterStage.Attending"
            type="dark"
            class="rounded-xl text-slate-900"
            size="sm"
          >
            <template #icon>
              <IconClockFastForward />
            </template>
            <span class="pl-2">{{ elapsedTime }}</span>
          </fwb-badge>
          <!-- Attending duration -->
          <fwb-badge
            v-if="encounter?.stage === EncounterStage.Charting"
            type="dark"
            class="rounded-xl text-slate-900"
            size="sm"
          >
            <template #icon>
              <IconClockCheck />
            </template>
            <span class="pl-2">{{ attendingDuration }}</span>
          </fwb-badge>
          <user-avatar
            v-if="props.encounter?.assignments"
            v-for="(assignment, index) in props.encounter.assignments"
            :key="assignment?.clinicianId || index"
            :user="getUserForAvatar(assignment?.clinicianId)"
            rounded
            size="sm"
          />
        </div>
      </div>
      <div v-if="!minimal" class="pt-4">
        <h4 class="text-sm font-semibold text-slate-900 flex items-center gap-2">
          <span>
            {{ props.job.contact }}
          </span>
          <fwb-badge v-if="props.encounter?.lines && externallyPlaced" class="bg-yellow-100 text-yellow-700">
            <template #icon>
              <IconArrowCircleBrokenDownRight />
            </template>
          </fwb-badge>
          <fwb-badge v-if="props.encounter?.lines && hasInfection" class="bg-radical-red-100 text-radical-red-700">
            <template #icon>
              <IconAlertHexagon />
            </template>
          </fwb-badge>
        </h4>
        <p v-if="props.job.notes?.[0]?.text" class="text-sm font-medium">
          {{ props.job.notes[0].text }}
        </p>
      </div>
      <div class="pt-4 flex justify-between items-center">
        <p class="text-xs font-medium">{{ relativeTimeJob }}</p>
        <fwb-badge
          v-if="props.job.status"
          size="xs"
          class="rounded-xl mr-0"
          :class="`text-${badgeColor}-700 bg-${badgeColor}-100`"
          >{{ props.job.status }}</fwb-badge
        >
      </div>
    </div>
  </div>
</template>
