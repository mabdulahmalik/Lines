import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import { EncounterAlertType, useBroadcastSubscription } from '@/api/__generated__/graphql';
import { useEncountersStore } from '../encounters';
import { useFacilitiesStore } from '../facilities';
import { usePurposesStore } from '../settings/purposes';
import { useUsersStore } from '../settings/users';

export const useNotificationsStore = defineStore('notifications', () => {
  provideApolloClient(client);

  const encountersStore = useEncountersStore();
  const facilitiesStore = useFacilitiesStore();
  const purposesStore = usePurposesStore();
  const usersStore = useUsersStore();

  const notifications = ref<any[]>([]);

  const processNotification = (payload: any) => {
    encountersStore.getEncounter(payload.encounterId)((encounterResult) => {
      const encounter = encounterResult?.data?.encounters?.items?.[0];
      const assistanceRequest = encounter?.alerts?.find(alert => alert?.type === EncounterAlertType.AssistanceRequested);

      if (!encounter || !assistanceRequest) return;

      facilitiesStore.getFacility(encounter.location?.facilityId)((facilityResult) => {
        const facility = facilityResult?.data?.facilities?.items?.[0];
        if (!facility) return;

        purposesStore.getPurpose(encounter.purposeId)((purposeResult) => {
          const purpose = purposeResult?.data?.purposes?.items?.[0];
          if (!purpose) return;

          facilitiesStore.getRoom(encounter.location?.roomId)((roomResult) => {
            const room = roomResult?.data?.facilityRooms?.items?.[0];
            if (!room) return;

            usersStore.getUser(payload.alertedBy)((usersResult) => {
              const user = usersResult?.data?.users?.items?.[0];
              if(!user) return;

              notifications.value.push({
                id: encounter.id,
                reason: assistanceRequest.text,
                facility: facility.name,
                room: room.name,
                purpose: purpose.name,
                user: user.firstName + ' ' + user.lastName,
                alertedBy: payload.alertedBy
              });

            })
          });
        });
      });
    });
  };

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    const broadcast = result.data?.broadcast;
    const payload = broadcast?.payload ? JSON.parse(broadcast.payload) : null;

    if (broadcast?.eventName === 'EncounterAlertAdded' && payload?.type === EncounterAlertType.AssistanceRequested) {
      processNotification(payload);
    }
  });

  const removeNotification = (id: any) => {
    notifications.value = notifications.value.filter(notification => notification.id !== id);
  };

  return {
    notifications,
    removeNotification,
  };
});
